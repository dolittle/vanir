// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend.Dolittle
{
    public class EventHorizonDefinition
    {
        public Guid Microservice;
        public Guid Tenant;
        public Guid Stream;
        public Guid Partition;
        public Guid Scope;
    }
}
