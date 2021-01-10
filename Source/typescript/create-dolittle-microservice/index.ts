#!/usr/bin/env node
/* eslint-disable header/header */

// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import fs from 'fs';
import commander from 'commander';
import chalk from 'chalk';
import { launchWizard } from './creation';
import { Globals } from './Globals';
export * from './Globals';
export * from './Config';

let root = __dirname;
if (root.endsWith('dist')) {
    root = path.join(root, '..');
}

const packageJsonFile = path.join(root, 'package.json');
let packageJson = { name: 'unknown', version: '1.0.0' };
if (fs.existsSync(packageJsonFile)) {
    packageJson = require(packageJsonFile);
}

const program = new commander.Command(packageJson.name)
    .version(packageJson.version)
    .arguments('<microservice-name>')
    .usage(`${chalk.green('<microservice-name>')} [options]`)
    .action(name => {
        Globals.rootPath = root;
        Globals.version = packageJson.version;

        launchWizard();
    })
    .addHelpCommand()
    .parse(process.argv);
