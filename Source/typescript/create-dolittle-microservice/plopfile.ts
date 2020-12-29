// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import { NodePlopAPI, ActionType, ActionConfig, AddManyActionConfig } from 'plop';
import rootPath from './packageRoot';
import { Answers } from 'inquirer';
import { Guid } from '@dolittle/rudiments';
import editJsonFile from 'edit-json-file';

const templatesRootPath = path.join(rootPath, 'templates', 'backend');
const webTemplatesRootPath = path.join(rootPath, 'templates', 'web');

const toPascalCase = function (input: string): string {
    return input.replace(/(\w)(\w*)/g,
        function (g0, g1, g2) { return g1.toUpperCase() + g2.toLowerCase(); });
};

function appendMicroserviceToApplication(answers: Answers, config?: ActionConfig, plopFileApi?: NodePlopAPI): string {
    const applicationFile = path.join(process.cwd(), 'application.json');
    const application = editJsonFile(applicationFile, { stringify_width: 4 });
    const microservices: string[] = application.get('microservices') as string[] | [];
    if (microservices.some(_ => _ === answers.path)) {
        throw new Error('A microservice with the given name is already added to the application.json file.');
    }

    microservices.push(answers.path);
    application.set('microservices', microservices);
    application.save();
    return 'Added microservice to application.json file';
}

export default function (plop: NodePlopAPI) {
    plop.setGenerator('microservice', {
        description: 'Creates a new Dolittle Microservice',
        prompts: [{
            type: 'input',
            name: 'name',
            message: 'Name of the microservice:',
            validate: (input) => input && input.length > 0
        }, {
            type: 'confirm',
            name: 'ui',
            message: 'Will the microservice have a UI?'
        }],
        actions: (answers?: Answers) => {
            const actions: ActionType[] = [];
            answers!.id = Guid.create().toString();
            answers!.name = toPascalCase(answers!.name);
            answers!.path = `./Source/${answers!.name}`;

            actions.push(appendMicroserviceToApplication);

            const applicationFile = path.join(process.cwd(), 'application.json');
            const application = require(applicationFile);
            answers!.applicationId = application.id;
            answers!.applicationName = application.name;
            answers!.tenant = application.tenant;
            answers!.license = application.license;
            answers!.uiPath = answers!.uiPath || `/_${answers!.name.toLowerCase()}`;

            actions.push({
                type: 'addMany',
                base: templatesRootPath,
                destination: path.join(process.cwd()),
                templateFiles: [
                    templatesRootPath,
                    path.join(templatesRootPath, '.*/**/*')
                ],
                stripExtensions: ['hbs']
            } as AddManyActionConfig);

            if (answers!.ui) {

                actions.push({
                    type: 'addMany',
                    base: webTemplatesRootPath,
                    destination: path.join(process.cwd()),
                    templateFiles: [
                        webTemplatesRootPath,
                        path.join(webTemplatesRootPath, '.*/**/*')
                    ],
                    stripExtensions: ['hbs']
                } as AddManyActionConfig);
            }

            return actions;
        }
    });
};
