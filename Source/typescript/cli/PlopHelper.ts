// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Answers } from 'inquirer';
import nodePlop from 'node-plop';
import out from 'plop/src/console-out';


const args = process.argv.slice(2);
export const argv = require('minimist')(args);
import ora from 'ora';

const progressSpinner = ora();

export interface PlopActionHooksFailures {
    type: string;
    path: string;
    error: string;
    message: string;
}

export interface PlopActionHooksChanges {
    type: string;
    path: string;
}


export class PlopHelper {

    constructor(private _file: string) { }

    async runGenerator(generatorName: string, answers: Answers, workingDirectory?: string): Promise<{
        changes: PlopActionHooksChanges[];
        failures: PlopActionHooksFailures[];
    }> {
        const plop = nodePlop(this._file, {
            force: true,
            destBasePath: workingDirectory || process.cwd()
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
            if (errMsg) { line += ` ${errMsg}`; };
            progressSpinner.fail(line); progressSpinner.start();
        };
        progressSpinner.start();

        const generator = plop.getGenerator(generatorName);
        const result = await generator.runActions(answers, {
            onComment,
            onFailure,
            onSuccess
        });

        progressSpinner.stop();

        return result;
    }
}
