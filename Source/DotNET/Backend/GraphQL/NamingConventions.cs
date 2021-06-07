// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Dolittle.Vanir.Backend.GraphQL
{
    public class NamingConventions : DefaultNamingConventions
    {
        public override NameString GetTypeName(Type type, TypeKind kind)
        {
            var name = base.GetTypeName(type, kind);
            if (kind == TypeKind.Enum &&
                !type.Namespace.StartsWith("HotChocolate", StringComparison.InvariantCultureIgnoreCase) &&
                !type.Namespace.StartsWith("Dolittle", StringComparison.InvariantCultureIgnoreCase) &&
                !name.Value.EndsWith("Enum", StringComparison.InvariantCultureIgnoreCase))
            {
                name = new NameString($"{name.Value}Enum");
            }

            return name;
        }
    }
}
