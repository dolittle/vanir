// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend.Dolittle
{
    public class EventHorizonDefinition
    {
        public Guid Microservice { get; set; }
        public Guid Tenant { get; set; }
        public Guid Stream { get; set; }
        public Guid Partition { get; set; }
        public Guid Scope { get; set; }
    }
}
