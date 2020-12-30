// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { NodePlopAPI, ActionType, ActionConfig, AddManyActionConfig } from 'plop';
import rootPath from './packageRoot';
import { Answers } from 'inquirer';
import { Guid } from '@dolittle/rudiments';

import { createMicroservice } from 'create-dolittle-microservice/dist/creation';

const templatesRootPath = path.join(rootPath, 'templates');

async function addPortalMicroservice(answers: Answers, config?: ActionConfig, plopFileApi?: NodePlopAPI): Promise<string> {
    await createMicroservice({
        name: 'portal',
        ui: true,
        portal: true
    });
    return 'Added portal microservice';
}

export default function (plop: NodePlopAPI) {
    plop.setGenerator('application', {
        description: 'Creates a new Dolittle Application',
        prompts: [{
            type: 'input',
            name: 'name',
            message: 'Name of the application:',
            validate: (input) => input && input.length > 0
        }, {
            type: 'input',
            name: 'tenant',
            message: 'Name of the tenant (company):',
            validate: (input) => input && input.length > 0
        }, {
            type: 'list',
            name: 'license',
            message: 'License to use:',
            default: 'MIT',
            choices: [
                { type: 'choice', name: 'MIT', value: 'MIT' },
                { type: 'choice', name: 'ISC', value: 'ISC' },
                { type: 'choice', name: 'BSD-3-Clause', value: 'BSD-3-Clause' },
                { type: 'choice', name: 'Apache-2.0', value: 'Apache-2.0' },
                { type: 'choice', name: 'GPL-3.0', value: 'GPL-3.0' },
                { type: 'choice', name: 'UNLICENSED', value: 'UNLICENSED' }
            ]
        }, {
            type: 'input',
            name: 'containerRegistry',
            message: 'Docker container registry (e.g. <something>.azurecr.io, hub.docker.com):',
            validate: (input) => input && input.length > 0
        }, {
            type: 'confirm',
            name: 'portal',
            message: 'Do you want a default UI microservice?'
        }],
        actions: (answers?: Answers) => {
            const actions: ActionType[] = [];
            answers!.id = Guid.create().toString();

            actions.push({
                type: 'addMany',
                base: templatesRootPath,
                destination: plop.getDestBasePath(),
                templateFiles: [
                    templatesRootPath,
                    path.join(templatesRootPath, '.*/**/*')
                ],
                stripExtensions: ['hbs']
            } as AddManyActionConfig);

            answers!.portalPath = '';

            if (answers?.portal) {
                actions.push(addPortalMicroservice);
                answers!.portalPath = './Source/Portal';
            }

            return actions;
        }
    });
};
