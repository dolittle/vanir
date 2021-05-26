// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { VisualEditor } from './VisualEditor';

export class ApplicationEditorProvider extends VisualEditor {
    private static readonly viewType = 'dolittle.application';

    public static register(context: vscode.ExtensionContext): vscode.Disposable {
        const provider = new ApplicationEditorProvider(context);
        const providerRegistration = vscode.window.registerCustomEditorProvider(ApplicationEditorProvider.viewType, provider);
        return providerRegistration;
    }

    constructor(context: vscode.ExtensionContext) {
        super(context);
    }

    get name(): string {
        return 'application';
    }

    async onInitialize(
        document: vscode.TextDocument,
        webviewPanel: vscode.WebviewPanel,
        token: vscode.CancellationToken
    ): Promise<void> {


        webviewPanel.webview.onDidReceiveMessage(e => {
            this.outputChannel.appendLine(e.type);
            vscode.window.showInformationMessage(e.type);
        });
    }
}
