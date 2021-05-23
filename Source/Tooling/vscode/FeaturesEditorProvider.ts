// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { getNonce } from './util';

export class FeaturesEditorProvider implements vscode.CustomTextEditorProvider {
    private static readonly viewType = 'dolittle.features';

    public static register(context: vscode.ExtensionContext): vscode.Disposable {
        const provider = new FeaturesEditorProvider(context);
        const providerRegistration = vscode.window.registerCustomEditorProvider(FeaturesEditorProvider.viewType, provider);
        return providerRegistration;
    }

    constructor(
        private readonly context: vscode.ExtensionContext
    ) { }

    async resolveCustomTextEditor(
        document: vscode.TextDocument,
        webviewPanel: vscode.WebviewPanel,
        token: vscode.CancellationToken
    ): Promise<void> {

        const content = document.getText();

        const channel = vscode.window.createOutputChannel('Dolittle Features');
        channel.appendLine('Starting custom text editor');
        webviewPanel.webview.options = {
            enableScripts: true
        };
        webviewPanel.webview.html = this.getHtmlForWebview(webviewPanel.webview);

        function updateWebview() {
            webviewPanel.webview.postMessage({
                type: 'update',
                text: document.getText(),
            });
        }

        webviewPanel.webview.onDidReceiveMessage(e => {
            updateWebview();
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
    }


    private getHtmlForWebview(webview: vscode.Webview): string {
        // Local path to script and css for the webview
        const scriptUri = webview.asWebviewUri(vscode.Uri.joinPath(
            this.context.extensionUri, 'dist/Visual/index.js'));

        const nonce = getNonce();

        return /* html */`
            <!DOCTYPE html>
            <html lang="en">

            <head>
                <meta charset="utf-8" />
                <meta name="viewport" content="width=device-width, initial-scale=1">
            </head>

            <body class="ms-Fabric">
                <div id="root"></div>
                <script nonce="${nonce}" src="${scriptUri}"></script>
            </body>

            </html>`;
    }
}
