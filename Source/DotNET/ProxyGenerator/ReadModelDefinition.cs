// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dolittle.Vanir.ProxyGenerator
{
    public class ReadModelDefinition : IProxyType, IEqualityComparer<ReadModelDefinition>
    {
        public string Name { get; init; }
        public string Namespace { get; init; }
        public string FilePathForImports { get; init; }

        public Type Type { get; init; }

        public IEnumerable<PropertyDefinition> Properties { get; init; }

        public bool Equals(ReadModelDefinition? x, ReadModelDefinition? y)
        {
            return x?.Type == y?.Type;
        }

        public int GetHashCode([DisallowNull] ReadModelDefinition obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}
