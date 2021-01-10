// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';

export class Globals {
    private static _version?: string;
    private static _rootPath?: string;

    static set version(version: string) {
        this._version = version;
    }

    static get version() {
        if (!this._version) {
            const packageJson = require(path.join(this.rootPath, 'package.json'));
            this._version = packageJson.version;
        }
        return this._version || '';
    }

    static set rootPath(path: string) {
        this._rootPath = path;
    }

    static get rootPath() {
        if (!this._rootPath) {
            let rootPath = path.dirname(require.resolve('create-dolittle-app'));
            if (rootPath.endsWith('dist')) {
                rootPath = path.join(rootPath, '..');
            }
            this._rootPath = rootPath;
        }
        console.log(`RootPath : ${this._rootPath}`);
        return this._rootPath;
    }
}
