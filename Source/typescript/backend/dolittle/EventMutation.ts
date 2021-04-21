// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Arg, Mutation, Resolver } from 'type-graphql';
import { Guid } from '@dolittle/rudiments';
import { IEventStore } from './IEventStore';

@Resolver()
export class EventMutationFor<T> {

    constructor(private readonly _eventStore: IEventStore) {
    }

    @Mutation(() => Number)
    async commit(@Arg('eventSourceId') eventSourceId: Guid, @Arg('event') event: T): Promise<Number> {
        const committedEvents = await this._eventStore.commit(event, eventSourceId);
        if (!committedEvents.failed) {
            const committedEvent = committedEvents.events.toArray()[0];
            return committedEvent.eventLogSequenceNumber.value;
        }

        return -1;
    }
}


