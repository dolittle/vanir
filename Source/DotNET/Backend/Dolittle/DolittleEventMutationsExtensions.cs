// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Dolittle.SDK.Events;
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.Reflection;
using Dolittle.Vanir.Backend.Strings;

namespace Dolittle.Vanir.Backend.Dolittle
{
    public static class DolittleEventMutationsExtensions
    {
        public static void AddEventsAsMutations(this SchemaRoute mutations, ITypes types)
        {
            var eventTypes = types.All.Where(_ => _.HasAttribute<EventTypeAttribute>());
            if (eventTypes.Any())
            {
                var events = new SchemaRoute("events", "events", "_events");
                mutations.AddChild(events);
                foreach (var eventType in eventTypes)
                {
                    var mutationType = typeof(EventMutationFor<>).MakeGenericType(eventType);
                    var method = mutationType.GetMethod("Commit", BindingFlags.Public | BindingFlags.Instance);
                    var @event = new SchemaRouteItem(method, eventType.Name.ToCamelCase());
                    events.AddItem(@event);
                }
            }
        }
    }
}
