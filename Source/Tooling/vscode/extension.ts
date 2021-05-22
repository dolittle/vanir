// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import * as vscode from 'vscode';
import { ApplicationEditorProvider } from './ApplicationEditorProvider';
import { FeaturesEditorProvider } from './FeaturesEditorProvider';

export function activate(context: vscode.ExtensionContext) {
    context.subscriptions.push(ApplicationEditorProvider.register(context));
    context.subscriptions.push(FeaturesEditorProvider.register(context));
}

export function deactivate() {}

