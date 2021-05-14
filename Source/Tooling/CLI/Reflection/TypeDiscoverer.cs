// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Dolittle.Vanir.CLI.Reflection
{
    /// <summary>
    /// Represents an implementation of <see cref="ITypeDiscoverer"/>.
    /// </summary>
    public class TypeDiscoverer : ITypeDiscoverer
    {
        /// <inheritdoc/>
        public Types GetAllTypesFromProjectReferencedAssemblies(string path)
        {
            var directory = Path.GetDirectoryName(path);
            var assembly = Assembly.LoadFrom(path);
            var dependencyModel = DependencyContext.Load(assembly);
            var assemblies = dependencyModel.RuntimeLibraries
                                .Where(_ => _.Type.Equals("project", StringComparison.InvariantCultureIgnoreCase))
                                .Select(_ =>
                                {
                                    var group = _.RuntimeAssemblyGroups[0];
                                    var file = group?.RuntimeFiles[0];
                                    if (file != default)
                                    {
                                        var filePath = Path.Combine(directory, file.Path);
                                        return Assembly.LoadFrom(filePath);
                                    }
                                    return null;
                                })
                                .Where(_ => _ != default)
                                .ToArray();

            return new Types(assemblies.SelectMany(asm => asm.DefinedTypes));
        }
    }
}
