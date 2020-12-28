#!/usr/bin/env node
/* eslint-disable header/header */

// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import fs from 'fs';
import commander from 'commander';
import chalk from 'chalk';
import { Plop, run } from 'plop';
import rootPath from './packageRoot';

const args = process.argv.slice(2);
const argv = require('minimist')(args);

const packageJsonFile = path.join(rootPath, 'package.json');
let packageJson = { name: 'unknown', version: '1.0.0' };
if (fs.existsSync(packageJsonFile)) {
    packageJson = require(packageJsonFile);
}

const cwd = process.cwd();
const plopFile = path.join(__dirname, 'plopfile.js');

const program = new commander.Command(packageJson.name)
    .version(packageJson.version)
    .arguments('<application-name>')
    .usage(`${chalk.green('<application.name>')} [options]`)
    .action(name => {
        Plop.launch({
            cwd,
            configPath: plopFile,
            require: argv.require,
            completion: argv.completion
        }, env => run(env, undefined, true));

    })
    .addHelpCommand()
    .parse(process.argv);
