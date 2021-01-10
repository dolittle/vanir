// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { Globals } from './Globals';

export class Config {
    private static _templatesRootPath?: string;

    static set templatesRootPath(path: string) {
        this._templatesRootPath = path;
    }

    static get templatesRootPath() {
        if (!this._templatesRootPath) {
            this._templatesRootPath = path.join(Globals.rootPath, 'templates');
        }
        return this._templatesRootPath;
    }
}
