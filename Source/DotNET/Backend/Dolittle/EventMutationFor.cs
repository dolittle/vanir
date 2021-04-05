// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.SDK.Events.Store;

namespace Dolittle.Vanir.Backend.Dolittle
{
    public class EventMutationFor<T>
    {
        readonly IEventStore _eventStore;

        public EventMutationFor(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<int> Commit(Guid eventSourceId, T @event)
        {
            var committedEvents = await _eventStore.CommitEvent(@event, eventSourceId);
            if (committedEvents.HasEvents)
            {
                var committedEvent = committedEvents.Single();
                return (int)committedEvent.EventLogSequenceNumber.Value;
            }

            return -1;
        }
    }
}
