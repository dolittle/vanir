// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { Plop, run } from 'plop';
import out from 'plop/src/console-out';
import nodePlop from 'node-plop';

const args = process.argv.slice(2);
export const argv = require('minimist')(args);
import ora from 'ora';

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

const progressSpinner = ora();

export async function createMicroservice(name: string, ui: boolean = true) {
    const plop = nodePlop(plopFile, {
        force: true,
        destBasePath: cwd
    });

    const noMap = (argv['show-type-names'] || argv.t);
    const onComment = (msg) => {
        progressSpinner.info(msg); progressSpinner.start();
    };
    const onSuccess = (change) => {
        let line = '';
        if (change.type) { line += ` ${out.typeMap(change.type, noMap)}`; }
        if (change.path) { line += ` ${change.path}`; }
        progressSpinner.succeed(line); progressSpinner.start();
    };
    const onFailure = (fail) => {
        let line = '';
        if (fail.type) { line += ` ${out.typeMap(fail.type, noMap)}`; }
        if (fail.path) { line += ` ${fail.path}`; }
        const errMsg = fail.error || fail.message;
        if (errMsg) { line += ` ${errMsg}` };
        progressSpinner.fail(line); progressSpinner.start();
    };
    progressSpinner.start();

    const generator = plop.getGenerator('microservice');
    const result = await generator.runActions({
        name,
        ui
    }, {
        onComment,
        onFailure,
        onSuccess
    });

    progressSpinner.stop();
}
