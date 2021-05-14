// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Dolittle.Vanir.CLI.GraphQL
{
    public class TypeDefinition : ISchemaType, IEqualityComparer<TypeDefinition>
    {
        public string Name { get; init; }
        public string Namespace { get; init; }
        public string FilePathForImports { get; init; }

        public Type Type { get; init; }

        public IEnumerable<PropertyDefinition> Properties { get; set; }

        public bool Equals(TypeDefinition x, TypeDefinition y)
        {
            return x?.Type == y?.Type;
        }

        public int GetHashCode([DisallowNull] TypeDefinition obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}
