// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export { IEventStore, IEventTypes } from '@dolittle/sdk.events';
export * from './EventStoreConfiguration';

import { constructor, containerInstance } from '@dolittle/vanir-dependency-inversion';

import { Client, ClientBuilder } from '@dolittle/sdk';
import { PartitionId, IEventStore, IEventTypes } from '@dolittle/sdk.events';
import { PartitionedFilterResult } from '@dolittle/sdk.events.filtering';
import { Logger } from 'winston';
import { container, DependencyContainer } from 'tsyringe';
import { Configuration } from '../Configuration';
import { logger } from '../logging';
import { IResourceConfigurations } from '../resources/IResourceConfigurations';
import { MongoDbReadModelsConfiguration } from '../mongodb/index';
import { EventStoreConfiguration } from '../dolittle';
import { getCurrentContext } from '../index';
import { Aggregate, IAggregate } from '../aggregates';
import { BackendArguments } from '../BackendArguments';
import { DolittleContainer } from './DolittleContainer';
import { EventHorizons } from './EventHorizons';

export type DolittleClientBuilderCallback = (clientBuilder: ClientBuilder) => void;

export async function initialize(configuration: Configuration, startArguments: BackendArguments): Promise<Client> {
    const clientBuilder = Client
        .forMicroservice(configuration.microserviceId)
        .withLogging(logger as Logger)
        .withContainer(new DolittleContainer(containerInstance))
        .withRuntimeOn(configuration.dolittle.runtime.host, configuration.dolittle.runtime.port)
        .withEventTypes(_ => startArguments.eventTypes?.forEach(et => _.register(et)))
        .withEventHandlers(_ => startArguments.eventHandlerTypes?.forEach(eh => _.register(eh)))
        .withProjections(_ => startArguments.projectionTypes?.forEach(pt => _.register(pt)))
        .withEventHorizons(_ => {
            const eventHorizons = EventHorizons.load();
            for (const tenant of eventHorizons.keys()) {
                _.forTenant(tenant, sb => {
                    for (const subscription of eventHorizons.get(tenant)!) {
                        sb
                            .fromProducerMicroservice(subscription.microservice)
                            .fromProducerTenant(subscription.tenant)
                            .fromProducerStream(subscription.stream)
                            .fromProducerPartition(subscription.partition)
                            .toScope(subscription.scope);
                    }
                });
            }
        })
        .withFilters(_ => {
            if (startArguments.publishAllPublicEvents !== false) {
                _.createPublicFilter(configuration.microserviceId, fb => fb
                    .handle((event, context) => {
                        return new PartitionedFilterResult(true, PartitionId.unspecified);
                    }));
            }
        })
        .useProjections(p => p.storeInMongoUsingProvider((eventContext) => {
            const resourceConfigurations = container.resolve(IResourceConfigurations as constructor<IResourceConfigurations>);
            const configuration = resourceConfigurations.getFor(MongoDbReadModelsConfiguration, eventContext.executionContext.tenantId);
            return {
                connectionString: configuration.host,
                databaseName: configuration.database
            };
        }))
        .useProjectionsIntermediates(p => p.storeInMongoUsingProvider((eventContext) => {
            const resourceConfigurations = container.resolve(IResourceConfigurations as constructor<IResourceConfigurations>);
            const configuration = resourceConfigurations.getFor(EventStoreConfiguration, eventContext.executionContext.tenantId);
            return {
                connectionString: configuration.servers[0],
                databaseName: configuration.database
            };
        }));

    startArguments.dolittleCallback?.(clientBuilder);

    const client = clientBuilder.build();
    container.register(IEventStore as constructor<IEventStore>, {
        useFactory: (dependencyContainer: DependencyContainer) => {
            return client.eventStore.forTenant(getCurrentContext().tenantId);
        }
    });

    container.registerInstance(Client, client);
    container.registerInstance(IEventTypes as constructor<IEventTypes>, client.eventTypes);
    container.register(IAggregate as constructor<IAggregate>, Aggregate);

    return client;
}
