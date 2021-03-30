// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Represents an implementation of <see cref="IGraphControllers"/>.
    /// </summary>
    public class GraphControllers : IGraphControllers
    {
        public GraphControllers()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            var dependencyModel = DependencyContext.Load(entryAssembly);
            var assemblies = dependencyModel.RuntimeLibraries
                                .Where(_ => _.Type.Equals("project", StringComparison.InvariantCultureIgnoreCase))
                                .Select(_ => Assembly.Load(_.Name))
                                .ToArray();

            var types = new List<Type>();
            var graphControllerType = typeof(GraphController);
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.DefinedTypes.Where(_ => _ != graphControllerType &&
                                                                _.IsAssignableTo(graphControllerType)).ToArray());
            }

            All = types;
        }
        public IEnumerable<Type> All { get; }
    }
}
