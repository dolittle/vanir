// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using HotChocolate.Types.Descriptors;

namespace Dolittle.Vanir.Backend.GraphQL
{
    public class TypeInspector : DefaultTypeInspector
    {
        public override IEnumerable<MemberInfo> GetMembers(System.Type type)
        {
            var members = base.GetMembers(type);
            return members.Where(_ => _ is PropertyInfo property && property.CanWrite);
        }
    }
}
