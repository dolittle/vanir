// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export * from './Configuration';
export * from './Host';
export * from './logging';
export * from './Context';
export * from './HostContext';

export * as data from './data';
export * as dolittle from './dolittle';
export * as logging from './logging';
export * as tsoa from './tsoa';
export * as mongodb from './mongodb';
export * as resources from './resources';
export * as web from './web';

export {
    GuidScalar
} from './data';

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
