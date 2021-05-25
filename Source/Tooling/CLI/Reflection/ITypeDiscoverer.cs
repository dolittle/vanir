// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Dolittle.Vanir.CLI.Reflection
{
    /// <summary>
    /// Defines a system for discovering types in different scenarios.
    /// </summary>
    public interface ITypeDiscoverer
    {
        /// <summary>
        /// Get all types from an assembly and its project references assemblies.
        /// </summary>
        /// <param name="path">Path to assembly to get for.</param>
        /// <returns>Collection of <see cref="TypeInfo"/>.</returns>
        Types GetAllTypesFromProjectReferencedAssemblies(string path);
    }
}
