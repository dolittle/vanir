// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.SDK.Events;

namespace Dolittle.Vanir.Backend.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AggregateRootAttribute : Attribute
    {
        public AggregateRootAttribute(string guid)
        {
            Id = guid;
        }

        public AggregateRootId Id { get; }
    }
}
