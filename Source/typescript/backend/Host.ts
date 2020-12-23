// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';

import path from 'path';
import dotenv from 'dotenv';

import * as Dolittle from './dolittle';
import { Mongoose } from './data';
import * as Express from './web';
import { configureLogging } from './logging/Logging';
import * as DependencyInversion from '@dolittle/dependency-inversion';

import '@dolittle/projections';
import { Configuration } from './Configuration';

export class Host {
    static async start(configuration: Configuration) {
        const envPath = path.resolve(process.cwd(), '.env');
        dotenv.config({ path: envPath });
        configureLogging(configuration.microserviceId);

        DependencyInversion.initialize();

        await Mongoose.initialize(configuration.defaultDatabaseName);
        await Dolittle.initialize(
            configuration.microserviceId,
            configuration.dolittleRuntimePort,
            configuration.defaultDatabaseName,
            configuration.defaultEventStoreDatabaseName,
            configuration.dolittleCallback);

        await Express.initialize(
            configuration.prefix,
            configuration.publicPath,
            configuration.port,
            configuration.graphQLSchema,
            configuration.expressCallback);
    }
}
