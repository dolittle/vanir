// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { constructor } from '@dolittle/vanir-dependency-inversion';
import { MongoClient } from 'mongodb';
import { container, DependencyContainer } from 'tsyringe';
import { IResourceConfigurations } from '../resources/IResourceConfigurations';
import { MongoDbReadModelsConfiguration } from './MongoDbReadModelsConfiguration';

import { getCurrentContext } from '../web';
import { IMongoDatabase } from './IMongoDatabase';
import { MongoDatabase } from './MongoDatabase';

export class MongoDb {

    static initialize() {
        container.register(MongoDbReadModelsConfiguration, {
            useFactory: (dependencyContainer: DependencyContainer) => {
                const resourceConfigurations = dependencyContainer.resolve(IResourceConfigurations as constructor<IResourceConfigurations>);
                return resourceConfigurations.getFor(MongoDbReadModelsConfiguration, getCurrentContext().tenantId);
            }
        });
        container.register(MongoClient, {
            useFactory: (dependencyContainer: DependencyContainer) => {
                const configuration = dependencyContainer.resolve(MongoDbReadModelsConfiguration);
                const client = new MongoClient(configuration.host, { useNewUrlParser: true, useUnifiedTopology: true });
                return client;
            }
        });

        container.register(IMongoDatabase as constructor<IMongoDatabase>, {
            useFactory: (dependencyContainer: DependencyContainer) => {
                const configuration = dependencyContainer.resolve(MongoDbReadModelsConfiguration);
                const client = dependencyContainer.resolve(MongoClient);
                const database = new MongoDatabase(client, configuration);
                return database;
            }
        });
    }
}
