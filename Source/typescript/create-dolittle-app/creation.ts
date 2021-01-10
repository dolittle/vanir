// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';

import { Plop, run } from 'plop';
import { Answers } from 'inquirer';
import { PlopHelper } from '@dolittle/vanir-cli';
const args = process.argv.slice(2);
export const argv = require('minimist')(args);
import plopConfigurator from './plopfile';

const cwd = process.cwd();
export const plopFile = path.join(__dirname, 'plopfile.js');

export function launchWizard() {
    Plop.launch({
        cwd,
        configPath: plopFile,
        require: argv.require,
        completion: argv.completion
    }, env => run(env, undefined, true));
}

export async function createApplication({ name, tenant, license, containerRegistry, portal, targetDirectory }: { name: string; tenant: string; license: string; containerRegistry: string; portal: boolean; targetDirectory?: string; }) {
    const helper = new PlopHelper(plopConfigurator);
    const answers = { name, tenant, license, containerRegistry, portal } as Answers;
    const result = await helper.runGenerator('application', answers, targetDirectory);
    return result;
}
