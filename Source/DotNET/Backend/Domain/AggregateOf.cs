// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Dolittle.SDK.Events;
using Dolittle.SDK.Events.Store;

namespace Dolittle.Vanir.Backend.Domain
{
    /// <summary>
    /// Represents an implementation of <see cref="IAggregateOf{T}"/>.
    /// </summary>
    /// <typeparam name="TAggregate">Type of <see cref="AggregateRoot"/>.</typeparam>
    public class AggregateOf<TAggregate> : IAggregateOf<TAggregate>
        where TAggregate : AggregateRoot
    {
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateOf{T}"/> class.
        /// </summary>
        /// <param name="eventStore">The <see cref="IEventStore" />.</param>
        public AggregateOf(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        /// <inheritdoc/>
        public IAggregateRootOperations<TAggregate> Get(EventSourceId eventSourceId)
        {
            var type = typeof(TAggregate);
            var constructor = GetConstructorFor(type);
            ThrowIfConstructorIsInvalid(type, constructor);

            var aggregateRoot = GetInstanceFrom(eventSourceId, constructor);
            if (aggregateRoot != null)
            {
                ReApplyEvents(aggregateRoot);
            }

            return new AggregateRootOperations<TAggregate>(_eventStore, aggregateRoot);
        }


        void ReApplyEvents(TAggregate aggregateRoot)
        {
            var eventSourceId = aggregateRoot.EventSourceId;
            var aggregateRootId = aggregateRoot.GetAggregateRootId();

            var committedEvents = _eventStore.FetchForAggregate(aggregateRootId, eventSourceId, CancellationToken.None).GetAwaiter().GetResult();
            if (committedEvents.HasEvents)
            {
                aggregateRoot.ReApply(committedEvents);
            }
        }

        TAggregate GetInstanceFrom(EventSourceId id, ConstructorInfo constructor)
        {
            return (constructor.GetParameters()[0].ParameterType == typeof(EventSourceId) ?
                constructor.Invoke(new object[] { id }) :
                constructor.Invoke(new object[] { id.Value })) as TAggregate;
        }

        ConstructorInfo GetConstructorFor(Type type)
        {
            return type.GetTypeInfo().GetConstructors().SingleOrDefault(c =>
            {
                var parameters = c.GetParameters();
                return parameters.Length == 1 &&
                    (parameters[0].ParameterType == typeof(Guid) ||
                        parameters[0].ParameterType == typeof(EventSourceId));
            });
        }

        void ThrowIfConstructorIsInvalid(Type type, ConstructorInfo constructor)
        {
            if (constructor == null) throw new InvalidAggregateRootConstructorSignature(type);
        }
    }
}
