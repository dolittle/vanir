// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Config;
using Dolittle.Vanir.Backend.Reflection;

namespace Dolittle.Vanir.Backend
{
    public class Services
    {
        public Configuration Configuration { get; init; }
        public IContainer Container { get; init; }
        public ITypes Types { get; init; }
    }
}
