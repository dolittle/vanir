// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export * from './IEventStore';
export * from './EventStoreConfiguration';

import { constructor, containerInstance } from '@dolittle/vanir-dependency-inversion';

import { Client, ClientBuilder } from '@dolittle/sdk';
import { Logger } from 'winston';
import { container, DependencyContainer } from 'tsyringe';
import { IEventStore } from './IEventStore';
import { IEventTypes } from './IEventTypes';
import { Configuration } from '../Configuration';
import { logger } from '../logging';
import { IResourceConfigurations } from '../resources/IResourceConfigurations';
import { MongoDbReadModelsConfiguration } from '../mongodb/index';
import { EventStoreConfiguration } from '../dolittle';
import { getCurrentContext } from '../index';

export type DolittleClientBuilderCallback = (clientBuilder: ClientBuilder) => void;


export async function initialize(configuration: Configuration, callback?: DolittleClientBuilderCallback): Promise<Client> {
    const clientBuilder = Client
        .forMicroservice(configuration.microserviceId)
        .withLogging(logger as Logger)
        .withContainer(containerInstance)
        .withRuntimeOn(configuration.dolittle.runtime.host, configuration.dolittle.runtime.port)
        .withProjections(p => p.storeInMongoUsingProvider((eventContext) => {
            const resourceConfigurations = container.resolve(IResourceConfigurations as constructor<IResourceConfigurations>);
            const configuration = resourceConfigurations.getFor(MongoDbReadModelsConfiguration, eventContext.executionContext.tenantId);
            return {
                connectionString: configuration.host,
                databaseName: configuration.database
            };
        }))
        .withProjectionIntermediates(p => p.storeInMongoUsingProvider((eventContext) => {
            const resourceConfigurations = container.resolve(IResourceConfigurations as constructor<IResourceConfigurations>);
            const configuration = resourceConfigurations.getFor(EventStoreConfiguration, eventContext.executionContext.tenantId);
            return {
                connectionString: configuration.servers[0],
                databaseName: configuration.database
            };
        }));

    callback?.(clientBuilder);

    const client = clientBuilder.build();
    container.register(IEventStore as constructor<IEventStore>, {
        useFactory: (dependencyContainer: DependencyContainer) => {
            return client.eventStore.forTenant(getCurrentContext().tenantId);
        }
    });

    container.registerInstance(IEventTypes as constructor<IEventTypes>, client.eventTypes);

    return client;
}
