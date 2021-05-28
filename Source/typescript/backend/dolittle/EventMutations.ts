// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GraphQLArgument, GraphQLInt, GraphQLBoolean } from 'graphql';
import { BackendArguments } from '../BackendArguments';
import { SchemaRoute } from '../graphql/SchemaRoute';
import { GuidScalar } from '../graphql/GuidScalar';
import { ObjectScalar } from '../graphql/ObjectScalar';
import { container } from 'tsyringe';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { IEventStore, IEventTypes, CommitEventsResult } from '@dolittle/sdk.events';
import { Guid } from '@dolittle/rudiments';

export class EventMutations {
    static addAllEvents(root: SchemaRoute, backendArguments: BackendArguments) {
        if (backendArguments.eventTypes) {
            const events = new SchemaRoute('events', 'events', '_events');
            root.addChild(events);

            for (const eventType of backendArguments.eventTypes) {
                const instance = new eventType();
                const properties = Object.getOwnPropertyNames(eventType.prototype);
                const eventName = `${eventType.name[0].toLowerCase()}${eventType.name.substr(1)}`;
                const args: GraphQLArgument[] = [
                    {
                        name: 'eventSourceId',
                        description: 'The event source identifier',
                        type: GuidScalar,
                        defaultValue: undefined,
                        deprecationReason: undefined,
                        extensions: {},
                        astNode: undefined
                    },
                    {
                        name: 'event',
                        description: 'The event content',
                        type: ObjectScalar,
                        defaultValue: undefined,
                        deprecationReason: undefined,
                        extensions: {},
                        astNode: undefined
                    },
                    {
                        name: 'isPublic',
                        description: 'Whether or not you want to commit a public event',
                        type: GraphQLBoolean,
                        defaultValue: false,
                        deprecationReason: undefined,
                        extensions: {},
                        astNode: undefined
                    }
                ];
                events.addItem({
                    name: eventName,
                    description: '',
                    deprecationReason: '',
                    isDeprecated: false,
                    extensions: {},
                    type: GraphQLInt,
                    args,
                    resolve: async (source, args: any, context, info): Promise<number> => {
                        const eventStore = container.resolve(IEventStore as constructor<IEventStore>);
                        const eventTypes = container.resolve(IEventTypes as constructor<IEventTypes>);
                        const eventTypeId = eventTypes.getFor(eventType);

                        let committedEvents: CommitEventsResult;

                        for (const property of Object.keys(args.event)) {
                            if (args.event[property] instanceof Guid) {
                                args.event[property] = args.event[property].toString();
                            }
                        }

                        if (args.isPublic) {
                            committedEvents = await eventStore.commitPublic(args.event, args.eventSourceId, eventTypeId);
                        } else {
                            committedEvents = await eventStore.commit(args.event, args.eventSourceId, eventTypeId);
                        }

                        if (!committedEvents.failed) {
                            const committedEvent = committedEvents.events.toArray()[0];
                            return committedEvent.eventLogSequenceNumber.value;
                        }

                        return -1;
                    }
                });
            }
        }
    }
}


