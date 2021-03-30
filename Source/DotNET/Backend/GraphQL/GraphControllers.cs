// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Dolittle.Vanir.Backend.Reflection;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Represents an implementation of <see cref="IGraphControllers"/>.
    /// </summary>
    public class GraphControllers : IGraphControllers
    {
        public GraphControllers(ITypes types)
        {
            All = types.FindMultiple<GraphController>();
        }

        /// <inheritdoc/>
        public IEnumerable<Type> All { get; }
    }
}
