// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { Plop, run } from 'plop';

const args = process.argv.slice(2);
export const argv = require('minimist')(args);
import { PlopHelper } from '@dolittle/vanir-cli';
import { Answers } from 'inquirer';
import plopConfigurator from './plopfile';

const cwd = process.cwd();
const plopFile = path.join(__dirname, 'plopfile.js');

export function launchWizard() {
    Plop.launch({
        cwd,
        configPath: plopFile,
        require: argv.require,
        completion: argv.completion
    }, env => run(env, undefined, false));
}

export async function createMicroservice({ name, ui = true, portal = false, targetDirectory, id }: { name: string; ui?: boolean; portal?: boolean; targetDirectory?: string; id?: string }) {
    const helper = new PlopHelper(plopConfigurator);
    const answers = { name, ui, id } as Answers;
    if (portal) {
        answers.hasUIPrefix = false;
    } else {
        answers.hasUIPrefix = true;
    }
    const result = await helper.runGenerator('microservice', answers, targetDirectory);
    return result;
}
