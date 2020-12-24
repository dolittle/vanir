#!/usr/bin/env node
/* eslint-disable header/header */

// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import commander from 'commander';
import chalk from 'chalk';
import { Plop, run } from 'plop';

const args = process.argv.slice(2);
const argv = require('minimist')(args);

let packageJson = { name: 'unknown', version: '1.0.0' };
if (require.main) {
    packageJson = require(path.join(require.main.path, 'package.json'));
}

console.log(require.main?.path);

const program = new commander.Command(packageJson.name)
    .version(packageJson.version)
    .arguments('<application-name>')
    .usage(`${chalk.green('<application.name>')} [options]`)
    .action(name => {
        const cwd = process.cwd();

        Plop.launch({
            cwd,
            configPath: path.join(__dirname, 'plopfile.js'),
            require: argv.require,
            completion: argv.completion
        }, env => run(env, undefined, true));
    })
    .addHelpCommand()
    .parse(process.argv);
