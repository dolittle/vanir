// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { getNonce } from './util';

export abstract class VisualEditor implements vscode.CustomTextEditorProvider {
    protected outputChannel!: vscode.OutputChannel;

    constructor(protected readonly context: vscode.ExtensionContext) {
    }

    abstract get name(): string;

    abstract onInitialize(document: vscode.TextDocument, webviewPanel: vscode.WebviewPanel, token: vscode.CancellationToken): Promise<void>;


    beforeUpdateDocumentInView(document: vscode.TextDocument): string {
        return document.getText();
    }

    async resolveCustomTextEditor(
        document: vscode.TextDocument,
        webviewPanel: vscode.WebviewPanel,
        token: vscode.CancellationToken
    ): Promise<void> {
        this.outputChannel = vscode.window.createOutputChannel(`Dolittle - ${this.name}`);

        webviewPanel.webview.options = {
            enableScripts: true
        };

        // eslint-disable-next-line @typescript-eslint/no-this-alias
        const self = this;
        function updateWebview() {
            webviewPanel.webview.postMessage({
                type: 'update',
                text: self.beforeUpdateDocumentInView(document),
            });
        }

        webviewPanel.webview.html = this.getHtmlForWebview(webviewPanel.webview);
        webviewPanel.webview.onDidReceiveMessage(async (e) => {
            switch (e.type) {
                case 'updateDocument': {
                    updateWebview();
                } break;
            }
        });

        const changeDocumentSubscription = vscode.workspace.onDidChangeTextDocument(e => {
            if (e.document.uri.toString() === document.uri.toString()) {
                updateWebview();
            }
        });

        webviewPanel.onDidDispose(() => {
            changeDocumentSubscription.dispose();
        });

        updateWebview();
        this.onInitialize(document, webviewPanel, token);
    }


    private getHtmlForWebview(webview: vscode.Webview): string {
        // Local path to script and css for the webview
        const scriptUri = webview.asWebviewUri(vscode.Uri.joinPath(
            this.context.extensionUri, 'dist/Visual/index.js'));

        const nonce = getNonce();

        return /* html */ `
            <!DOCTYPE html>
            <html>

            <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <meta name="editor" content="${this.name}">
            </head>

            <body class="ms-Fabric">
                <div id="root"></div>
                <script nonce="${nonce}" src="${scriptUri}"></script>
            </body>

            </html>`;
    }
}
