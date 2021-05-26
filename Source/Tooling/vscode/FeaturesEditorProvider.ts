// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { VisualEditor } from './VisualEditor';

export class FeaturesEditorProvider extends VisualEditor {
    private static readonly viewType = 'dolittle.features';

    public static register(context: vscode.ExtensionContext): vscode.Disposable {
        const provider = new FeaturesEditorProvider(context);
        const providerRegistration = vscode.window.registerCustomEditorProvider(FeaturesEditorProvider.viewType, provider);
        return providerRegistration;
    }

    constructor(context: vscode.ExtensionContext) {
        super(context);
    }

    get name(): string {
        return 'features';
    }

    async onInitialize(
        document: vscode.TextDocument,
        webviewPanel: vscode.WebviewPanel,
        token: vscode.CancellationToken
    ): Promise<void> {


        webviewPanel.webview.onDidReceiveMessage(async e => {
            switch (e.type) {
                case 'documentChanged': {
                    const edit = new vscode.WorkspaceEdit();
                    edit.replace(
                        document.uri,
                        new vscode.Range(0, 0, document.lineCount, 0),
                        e.data
                    );

                    await vscode.workspace.applyEdit(edit);
                    await document.save();
                } break;
            }
        });
    }
}
