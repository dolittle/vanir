// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HotChocolate.Types;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Extension methods for working with <see cref="IObjectTypeDescriptor{T}"/>.
    /// </summary>
    public static class ObjectTypeExtensions
    {
        /// <summary>
        /// Add all query fields based on <see cref="QueryAttribute"/> of members of a given type.
        /// </summary>
        /// <param name="descriptor"><see cref="IObjectTypeDescriptor{T}"/> to add to.</param>
        /// <param name="type"><see cref="Type"/> to add from</param>
        /// <returns>Any field descriptors added</returns>
        public static IEnumerable<IObjectFieldDescriptor> AddQueryFields(this IObjectTypeDescriptor<Query> descriptor, Type type)
        {
            return descriptor.AddFieldsFor<Query, QueryAttribute>(type);
        }

        /// <summary>
        /// Add all query fields based on <see cref="MuttationAttribute"/> of members of a given type.
        /// </summary>
        /// <param name="descriptor"><see cref="IObjectTypeDescriptor{T}"/> to add to.</param>
        /// <param name="type"><see cref="Type"/> to add from</param>
        /// <returns>Any field descriptors added</returns>
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
