// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { Constructor } from '@dolittle/types';
import { IAggregate } from './IAggregate';
import {
    AggregateRootVersionIsOutOfOrder,
    EventWasAppliedByOtherAggregateRoot,
    EventWasAppliedToOtherEventSource,
    EventSourceId,
    CommittedAggregateEvent,
    CommittedAggregateEvents,
    EventTypeId
} from '@dolittle/sdk.events';

import {
    AggregateRoot,
    IAggregateRootOperations,
    AggregateRootOperations,
    AggregateRootDecoratedTypes,
    OnDecoratedMethod,
    OnDecoratedMethods
} from '@dolittle/sdk.aggregates';

import { Guid } from '@dolittle/rudiments';
import { ILogger } from '../logging/ILogger';
import { IEventTypes, IEventStore } from '../dolittle';
import { Logger } from 'winston';

@injectable()
export class Aggregate implements IAggregate {
    constructor(private readonly _eventStore: IEventStore, private readonly _eventTypes: IEventTypes, private readonly _logger: ILogger) { }

    async of<TAggregate extends AggregateRoot>(type: Constructor<TAggregate>, eventSourceId: EventSourceId | Guid | string): Promise<IAggregateRootOperations<TAggregate>> {
        eventSourceId = EventSourceId.from(eventSourceId as any);
        const aggregateRoot = new type(eventSourceId);
        const aggregateRootId = AggregateRootDecoratedTypes.getFor(type).aggregateRootId;
        (aggregateRoot as any).aggregateRootId = aggregateRootId;

        this._logger.debug(
            `Re-applying events for ${type.name} with aggregate root id ${aggregateRootId} with event source id ${eventSourceId}`,
            type,
            aggregateRootId,
            eventSourceId
        );

        const committedEvents = await this._eventStore.fetchForAggregate(aggregateRootId, eventSourceId);
        if (committedEvents.hasEvents) {
            this._logger.silly(`Re-applying ${committedEvents.length}`, committedEvents.length);
            this.throwIfEventWasAppliedToOtherEventSource(aggregateRoot, committedEvents);
            this.throwIfEventWasAppliedByOtherAggreateRoot(aggregateRoot, committedEvents);

            let onMethods: OnDecoratedMethod[] = [];
            const hasState = OnDecoratedMethods.methodsPerAggregate.has(type);
            if (hasState) {
                onMethods = OnDecoratedMethods.methodsPerAggregate.get(type)!;
            }

            for (const event of committedEvents) {
                this.throwIfAggregateRootVersionIsOutOfOrder(aggregateRoot, event);
                aggregateRoot.nextVersion();
                if (hasState) {
                    const onMethod = onMethods.find(_ => {
                        let eventTypeId = EventTypeId.from(Guid.empty);
                        if (_.eventTypeOrId instanceof Function) {
                            eventTypeId = this._eventTypes.getFor(_.eventTypeOrId).id;
                        } else {
                            eventTypeId = EventTypeId.from(_.eventTypeOrId);
                        }

                        return eventTypeId.equals(event.type.id);
                    });

                    if (onMethod) {
                        onMethod.method.call(aggregateRoot, event.content);
                    }
                }
            }
        } else {
            this._logger.silly('No events to re-apply');
        }

        const operations = new AggregateRootOperations<TAggregate>(
            type,
            this._eventStore,
            aggregateRoot,
            this._eventTypes,
            this._logger as Logger);
        return operations;

    }

    private throwIfAggregateRootVersionIsOutOfOrder(aggregateRoot: AggregateRoot, event: CommittedAggregateEvent) {
        if (event.aggregateRootVersion.value !== aggregateRoot.version.value) {
            throw new AggregateRootVersionIsOutOfOrder(event.aggregateRootVersion, aggregateRoot.version);
        }
    }

    private throwIfEventWasAppliedByOtherAggreateRoot(aggregateRoot: AggregateRoot, event: CommittedAggregateEvents) {
        if (!event.aggregateRootId.equals(aggregateRoot.aggregateRootId)) {
            throw new EventWasAppliedByOtherAggregateRoot(event.aggregateRootId, aggregateRoot.aggregateRootId);
        }
    }

    private throwIfEventWasAppliedToOtherEventSource(aggregateRoot: AggregateRoot, event: CommittedAggregateEvents) {
        if (!event.eventSourceId.equals(aggregateRoot.eventSourceId)) {
            throw new EventWasAppliedToOtherEventSource(event.eventSourceId, aggregateRoot.eventSourceId);
        }
    }

}
