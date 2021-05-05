// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import 'reflect-metadata';
import { Host } from '@dolittle/vanir-backend';
import { RegisterRoutes } from './routes';
const swaggerDoc = require('./swagger.json');
import * as applications from './applications';
import { Bakery } from './groceryStore/bakery/Bakery';
import { NoQueries } from './NoQueries';
import { MyEvent } from './MyEvent';

(async () => {
    await Host.start({
        swaggerDoc,
        graphQLResolvers: [
            ...applications.CommandHandlers,
            ...applications.Queries,
            Bakery,
            NoQueries
        ],
        eventTypes: [
            ...applications.EventTypes,
            MyEvent
        ],
        eventHandlerTypes: [
            ...applications.EventHandlers
        ],
        expressCallback: (app) => {
            RegisterRoutes(app);
        }
    });
})();
