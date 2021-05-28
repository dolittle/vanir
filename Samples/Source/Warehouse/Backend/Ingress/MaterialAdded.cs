// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.SDK.Events;

namespace Backend.Ingress
{
    [EventType("72fe8ea4-6807-4ae3-a0a9-ba83504dd3ce")]
    public class MaterialAdded
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }
}
