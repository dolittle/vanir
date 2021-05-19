// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using HotChocolate.Utilities;

namespace Dolittle.Vanir.Backend.GraphQL
{
    public class EnumToIntConverter : IChangeTypeProvider
    {
        readonly Type _enumType;

        public EnumToIntConverter(Type enumType)
        {
            _enumType = enumType;
        }

        public bool TryCreateConverter(Type source, Type target, ChangeTypeProvider root, [NotNullWhen(true)] out ChangeType converter)
        {
            converter = (source) => (int)source;
            return source == _enumType;
        }
    }
}
