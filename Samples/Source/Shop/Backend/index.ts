// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';
import { Host } from '@dolittle/vanir-backend';
import { RegisterRoutes } from './routes';
const swaggerDoc = require('./swagger.json');
import * as cart from './cart';

(async () => {
    await Host.start({
        swaggerDoc,
        graphQLResolvers: [
            cart.Mutations,
            cart.Queries
        ],
        eventTypes: [
            ...cart.Events
        ],
        eventHandlerTypes: [
        ],
        expressCallback: (app) => {
            RegisterRoutes(app);
        }
    });
})();
