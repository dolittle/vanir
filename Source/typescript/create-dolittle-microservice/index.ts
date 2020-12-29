#!/usr/bin/env node
/* eslint-disable header/header */

// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import path from 'path';
import fs from 'fs';
import commander from 'commander';
import chalk from 'chalk';
import rootPath from './packageRoot';
import { launchWizard } from './wizard';

const packageJsonFile = path.join(rootPath, 'package.json');
let packageJson = { name: 'unknown', version: '1.0.0' };
if (fs.existsSync(packageJsonFile)) {
    packageJson = require(packageJsonFile);
}

const program = new commander.Command(packageJson.name)
    .version(packageJson.version)
    .arguments('<microservice-name>')
    .usage(`${chalk.green('<microservice-name>')} [options]`)
    .action(name => {
        launchWizard();
    })
    .addHelpCommand()
    .parse(process.argv);
