// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { getNonce } from './util';

export class ApplicationEditorProvider implements vscode.CustomTextEditorProvider {
    private static readonly viewType = 'dolittle.application';

    public static register(context: vscode.ExtensionContext): vscode.Disposable {
        const provider = new ApplicationEditorProvider(context);
        const providerRegistration = vscode.window.registerCustomEditorProvider(ApplicationEditorProvider.viewType, provider);
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

        const channel = vscode.window.createOutputChannel('Dolittle Application');
        channel.appendLine('Starting custom text editor');
        webviewPanel.webview.options = {
            enableScripts: true
        };
        webviewPanel.webview.html = this.getHtmlForWebview(webviewPanel.webview);

        webviewPanel.webview.onDidReceiveMessage(e => {
            channel.appendLine(e.type);
            vscode.window.showInformationMessage(e.type);
        });

        function updateWebview() {
            webviewPanel.webview.postMessage({
                type: 'update',
                text: document.getText(),
            });
        }

        updateWebview();
    }


    private getHtmlForWebview(webview: vscode.Webview): string {
        // Local path to script and css for the webview
        const scriptUri = webview.asWebviewUri(vscode.Uri.joinPath(
            this.context.extensionUri, 'dist/Visual/index.js'));

        const nonce = getNonce();

        return /* html */`
            <!DOCTYPE html>
            <html>

            <head>
                <meta charset="utf-8" />
                <title>Dolittle Application</title>
                <meta name="viewport" content="width=device-width, initial-scale=1">
            </head>

            <body class="ms-Fabric">
                <div id="root"></div>
                <script nonce="${nonce}" src="${scriptUri}"></script>
            </body>

            </html>`;
    }
}
