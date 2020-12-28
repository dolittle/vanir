// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { NodePlopAPI, ActionType } from 'plop';
import rootPath from './packageRoot';
import { Answers } from 'inquirer';
import { Guid } from '@dolittle/rudiments';

const templatePath = path.join(rootPath, 'templates', 'application.json');

export default function (plop: NodePlopAPI) {
    plop.setGenerator('application', {
        description: '',
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
            answers!.id = Guid.create().toString();
            return [{
                type: 'add',
                path: path.join(process.cwd(), 'application.json'),
                templateFile: templatePath
            }];
        }
    });
};
