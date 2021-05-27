// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.CLI.EventHorizon
{
    public class EventHorizon
    {
        public Guid Microservice;
        public Guid Tenant;
        public Guid Stream;
        public Guid Partition;
        public Guid Scope;
    }
}
