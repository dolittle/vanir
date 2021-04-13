// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Guid } from '@dolittle/rudiments';
import { EventType } from '@dolittle/sdk.artifacts';
import { Cancellation } from '@dolittle/sdk.resilience';

import {
    IEventStore as IActualEventStore,
    UncommittedEvent,
    CommitEventsResult,
    UncommittedAggregateEvents,
    CommitAggregateEventsResult,
    CommitForAggregateBuilder,
    CommittedAggregateEvents,
    AggregateRootId,
    AggregateRootVersion,
    EventTypeId,
    EventSourceId,

} from '@dolittle/sdk.events';

export abstract class IEventStore implements IActualEventStore {
    /** @inheritdoc */
    abstract commit(event: any, eventSourceId: EventSourceId | Guid | string, eventType?: EventType | EventTypeId | Guid | string, cancellation?: Cancellation): Promise<CommitEventsResult>;

    /** @inheritdoc */
    abstract commit(eventOrEvents: UncommittedEvent | UncommittedEvent[], cancellation?: Cancellation): Promise<CommitEventsResult>;

    /** @inheritdoc */
    abstract commitPublic(event: any, eventSourceId: EventSourceId |Guid | string, eventType?: EventType | Guid | string, cancellation?: Cancellation): Promise<CommitEventsResult>;

    /** @inheritdoc */
    abstract commitForAggregate(event: any, eventSourceId: EventSourceId | Guid | string, aggregateRootId: AggregateRootId, expectedAggregateRootVersion: AggregateRootVersion, eventType?: EventType | EventTypeId | Guid | string, cancellation?: Cancellation): Promise<CommitAggregateEventsResult>;

    /** @inheritdoc */
    abstract commitForAggregate(events: UncommittedAggregateEvents, cancellation?: Cancellation): Promise<CommitAggregateEventsResult>;

    /** @inheritdoc */
    abstract forAggregate(aggregateRootId: AggregateRootId): CommitForAggregateBuilder;

    /** @inheritdoc */
    abstract fetchForAggregate(aggregateRootId: AggregateRootId, eventSourceId: EventSourceId, cancellation?: Cancellation): Promise<CommittedAggregateEvents>;

    /** @inheritdoc */
    abstract fetchForAggregateSync(aggregateRootId: AggregateRootId, eventSourceId: EventSourceId, cancellation?: Cancellation): CommittedAggregateEvents;
}


