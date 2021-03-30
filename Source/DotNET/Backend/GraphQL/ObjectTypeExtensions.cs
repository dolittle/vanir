// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HotChocolate.Types;

namespace Dolittle.Vanir.Backend.GraphQL
{
    public static class ObjectTypeExtensions
    {
        public static IEnumerable<IObjectFieldDescriptor> AddQueryFields(this IObjectTypeDescriptor<Query> descriptor, Type type)
        {
            return descriptor.AddFieldsFor<Query, QueryAttribute>(type);
        }

        public static IEnumerable<IObjectFieldDescriptor> AddMutationFields(this IObjectTypeDescriptor<Mutation> descriptor, Type type)
        {
            return descriptor.AddFieldsFor<Mutation, MutationAttribute>(type);
        }

        static IEnumerable<IObjectFieldDescriptor> AddFieldsFor<TTarget, TAttribute>(this IObjectTypeDescriptor<TTarget> descriptor, Type type)
            where TAttribute : Attribute, ICanHaveName
        {
            var fields = new List<IObjectFieldDescriptor>();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(_ => _.GetCustomAttribute<TAttribute>() != null);
            foreach (var method in methods)
            {
                var fieldDescriptor = descriptor.Field(method);
                var queryAttribute = method.GetCustomAttribute<TAttribute>();
                if (queryAttribute?.HasName == true)
                {
                    fieldDescriptor.Name(queryAttribute.Name);
                }

                fields.Add(fieldDescriptor);
            }

            return fields;
        }
    }
}
