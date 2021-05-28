// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.SDK.Aggregates;
using Dolittle.SDK.Events;

namespace Backend.Ingress
{
    public class Receipt : AggregateRoot
    {
        public Receipt(EventSourceId eventSourceId) : base(eventSourceId) { }
    }
}
