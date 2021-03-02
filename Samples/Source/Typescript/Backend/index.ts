// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';
import { Host } from '@dolittle/vanir-backend';
import { RegisterRoutes } from './routes';
import { NoQueries } from './NoQueries';
const swaggerDoc = require('./swagger.json');


(async () => {
    await Host.start({
        swaggerDoc,
        graphQLResolvers: [NoQueries],
        expressCallback: (app) => {
            RegisterRoutes(app);
        }
    });
})();

