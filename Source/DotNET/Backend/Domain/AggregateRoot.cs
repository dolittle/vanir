// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Dolittle.SDK.Events;
using Dolittle.SDK.Events.Store;

namespace Dolittle.Vanir.Backend.Domain
{
    public class AggregateRoot
    {
        private readonly List<object> _events = new List<object>();
        private readonly EventSourceId _eventSourceId;

        public AggregateRoot(EventSourceId eventSourceId)
        {
            _eventSourceId = eventSourceId;

            Version = AggregateRootVersion.Initial;
        }

        public AggregateRootVersion Version { get; private set; }

        public EventSourceId EventSourceId => _eventSourceId;

        public IEnumerable<object> Events => _events;

        public void Apply(object @event)
        {
            _events.Add(@event);
            InvokeOnMethod(@event);
        }

        /// <summary>
        /// Re-apply events from the Event Store.
        /// </summary>
        /// <param name="events">Sequence that contains the events to re-apply.</param>
        public virtual void ReApply(CommittedAggregateEvents events)
        {
            foreach (var @event in events)
            {
                InvokeOnMethod(@event.Content);
                Version++;
            }
        }

        void InvokeOnMethod(object @event)
        {
            var handleMethod = this.GetOnMethod(@event);
            handleMethod?.Invoke(this, new[] { @event });
        }
    }
}
