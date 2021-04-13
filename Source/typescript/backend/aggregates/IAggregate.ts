// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AggregateRoot, IAggregateRootOperations } from '@dolittle/sdk.aggregates';
import { Constructor } from '@dolittle/types';
import { EventSourceId } from '@dolittle/sdk.events';
import { Guid } from '@dolittle/rudiments';

export abstract class IAggregate {
    abstract of<TAggregate extends AggregateRoot>(type: Constructor<TAggregate>, eventSourceId: EventSourceId | Guid | string): Promise<IAggregateRootOperations<TAggregate>>;
}


