// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { constructor } from '@dolittle/vanir-dependency-inversion';
import { MongoClient } from 'mongodb';
import { container, DependencyContainer } from 'tsyringe';
import { IResourceConfigurations } from '../resources/IResourceConfigurations';
import { MongoDbReadModelsConfiguration } from './MongoDbReadModelsConfiguration';

import { getCurrentContext } from '../index';
import { IMongoDatabase } from './IMongoDatabase';
import { MongoDatabase } from './MongoDatabase';
import { MongoDatabaseProvider } from './MongoDatabaseProvider';


const clientsPerTenant: Map<string, MongoClient> = new Map();

export class MongoDb {

    static initialize() {
        container.registerSingleton(MongoDatabaseProvider);

        container.register(MongoDbReadModelsConfiguration, {
            useFactory: (dependencyContainer: DependencyContainer) => {
                const resourceConfigurations = dependencyContainer.resolve(IResourceConfigurations as constructor<IResourceConfigurations>);
                return resourceConfigurations.getFor(MongoDbReadModelsConfiguration, getCurrentContext().tenantId);
            }
        });
        container.register(MongoClient, {
            useFactory: (dependencyContainer: DependencyContainer) => {
                const tenantId =  getCurrentContext().tenantId.toString();
                if(clientsPerTenant.has(tenantId)) {
                    return clientsPerTenant.get(tenantId);
                }

                const configuration = dependencyContainer.resolve(MongoDbReadModelsConfiguration);
                const client = new MongoClient(configuration.host, { useNewUrlParser: true, useUnifiedTopology: true });
                clientsPerTenant.set(tenantId, client);
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
