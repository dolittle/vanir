// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

module.exports = function (plop) {
    plop.setGenerator('test', {
        prompts: [{
            type: 'confirm',
            name: 'wantTacos',
            message: 'Do you want tacos?'
        }],
        actions: (data) => {
            const actions = [];

            if (data.wantTacos) {
                actions.push({
                    type: 'add',
                    path: 'folder/{{dashCase name}}.txt',
                    templateFile: 'templates/tacos.txt'
                });
            } else {
                actions.push({
                    type: 'add',
                    path: 'folder/{{dashCase name}}.txt',
                    templateFile: 'templates/burritos.txt'
                });
            }

            return actions;
        }
    });
};