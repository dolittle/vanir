// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { Plop, NodePlopAPI, ActionType, ActionConfig, AddManyActionConfig } from 'plop';
import rootPath from './packageRoot';
import { Answers } from 'inquirer';
import { Guid } from '@dolittle/rudiments';

import { createMicroservice } from 'create-dolittle-microservice/dist/wizard';

const templatesRootPath = path.join(rootPath, 'templates');

async function addPortalMicroservice(answers: Answers, config?: ActionConfig, plopFileApi?: NodePlopAPI): Promise<string> {
    await createMicroservice('portal', true);
    return 'Added portal microservice';
}

export default function (plop: NodePlopAPI) {
    plop.setGenerator('application', {
        description: 'Creates a new Dolittle Application',
        prompts: [{
            type: 'input',
            name: 'name',
            message: 'Name of the application:'
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
                destination: path.join(process.cwd()),
                templateFiles: [
                    templatesRootPath,
                    path.join(templatesRootPath, '.*')
                ],
                stripExtensions: ['hbs']
            } as AddManyActionConfig);

            if (answers?.portal) {
                actions.push(addPortalMicroservice);
            }

            return actions;
        }
    });
};
