// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { NodePlopAPI, Plop, run } from 'plop';
import nodePlop from 'node-plop';

const args = process.argv.slice(2);
export const argv = require('minimist')(args);

const cwd = process.cwd();
export const plopFile = path.join(__dirname, 'plopfile.js');

export function launchWizard() {
    Plop.launch({
        cwd,
        configPath: plopFile,
        require: argv.require,
        completion: argv.completion
    }, env => run(env, undefined, false));
}

export function createMicroservice(name: string, ui: boolean = true) {
    const plop = nodePlop(plopFile, {
        force: false,
        destBasePath: cwd
    })

    const generator = plop.getGenerator('microservice');
    generator.runActions({
        name,
        ui
    });
}
