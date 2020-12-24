// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';

import path from 'path';
import dotenv from 'dotenv';

import * as Dolittle from './dolittle';
import { Mongoose } from './data';
import * as Express from './web';
import * as DependencyInversion from '@dolittle/dependency-inversion';

export { logger } from './logging';

import '@dolittle/projections';
import { Configuration } from './Configuration';
import { BackendArguments } from './BackendArguments';
import { container } from 'tsyringe';

export class Host {
    static async start(startArguments: BackendArguments) {
        const envPath = path.resolve(process.cwd(), '.env');
        dotenv.config({ path: envPath });
        const configuration = Configuration.create();

        DependencyInversion.initialize();

        container.registerInstance(Configuration, configuration);

        await Mongoose.initialize(configuration);
        await Dolittle.initialize(configuration, startArguments.dolittleCallback);
        await Express.initialize(configuration, startArguments.graphQLResolvers, startArguments.swaggerDoc, startArguments.expressCallback);
    }
}
