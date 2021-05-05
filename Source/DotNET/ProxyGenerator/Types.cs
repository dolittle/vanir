// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Dolittle.Vanir.ProxyGenerator
{
    public static class Types
    {
        public static IEnumerable<TypeInfo> GetAllTypesFromProjectReferencedAssemblies(string path, string directory)
        {
            var assembly = Assembly.LoadFrom(path);
            var dependencyModel = DependencyContext.Load(assembly);
            var assemblies = dependencyModel.RuntimeLibraries
                                .Where(_ => _.Type.Equals("project", StringComparison.InvariantCultureIgnoreCase))
                                .Select(_ =>
                                {
                                    var group = _.RuntimeAssemblyGroups.FirstOrDefault();
                                    var file = group?.RuntimeFiles.FirstOrDefault();
                                    if (file != default)
                                    {
                                        var filePath = Path.Combine(directory, file.Path);
                                        return Assembly.LoadFrom(filePath);
                                    }
                                    return null;
                                })
                                .Where(_ => _ != default)
                                .ToArray();

            return assemblies.SelectMany(asm => asm.DefinedTypes);
        }
    }
}
