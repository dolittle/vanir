// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export * from './Configuration';
export * from './Host';
export * from './logging';
export * from './Context';
export * from './HostContext';

import * as data from './data';
import * as dolittle from './dolittle';
import * as logging from './logging';
import * as tsoa from './tsoa';
import * as mongodb from './mongodb';
import * as resources from './resources';
import * as web from './web';

export {
    data,
    dolittle,
    logging,
    mongodb,
    tsoa,
    resources,
    web
};

export {
    IEventStore,
    IEventTypes
} from './dolittle';

export {
    ILogger
} from './logging';

export {
    IMongoDatabase,
    MongoDatabaseProvider,
    MongoDbReadModelsConfiguration,
    guid
} from './mongodb';

export {
    IResourceConfigurations
} from './resources';
