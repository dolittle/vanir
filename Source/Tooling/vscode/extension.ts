// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { ApplicationEditorProvider } from './ApplicationEditorProvider';

export function activate(context: vscode.ExtensionContext) {
    context.subscriptions.push(ApplicationEditorProvider.register(context));
}

export function deactivate() {}
