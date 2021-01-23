// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Dolittle.SDK.Events.Store;

namespace Dolittle.Vanir.Backend.Domain
{
    /// <summary>
    /// Represents an implementation of <see cref="IAggregateRootOperations{T}"/>.
    /// </summary>
    /// <typeparam name="TAggregate"><see cref="AggregateRoot"/> type.</typeparam>
    public class AggregateRootOperations<TAggregate> : IAggregateRootOperations<TAggregate>
        where TAggregate : AggregateRoot
    {
        readonly TAggregate _aggregateRoot;
        readonly IEventStore _eventStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRootOperations{TAggregate}"/> class.
        /// </summary>
        /// <param name="aggregateRoot"><see cref="AggregateRoot"/> the operations are for.</param>
        public AggregateRootOperations(IEventStore eventStore, TAggregate aggregateRoot)
        {
            _aggregateRoot = aggregateRoot;
            _eventStore = eventStore;
        }

        /// <inheritdoc/>
        public async Task<AggregateRootPerformResult> Perform(Action<TAggregate> method)
        {
            method(_aggregateRoot);

            var aggregateRootId = _aggregateRoot.GetAggregateRootId();

            var commitContext = await _eventStore
                    .ForAggregate(aggregateRootId)
                    .WithEventSource(_aggregateRoot.EventSourceId)
                    .ExpectVersion(_aggregateRoot.Version)
                    .Commit(events =>
                    {
                        foreach (var @event in _aggregateRoot.Events)
                        {
                            events.CreateEvent(@event);
                        }
                    }).ConfigureAwait(false);

            var result = new AggregateRootPerformResult();
            return result;
        }
    }
}
