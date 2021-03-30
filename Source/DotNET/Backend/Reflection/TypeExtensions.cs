// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dolittle.Vanir.Backend.Reflection
{
    /// <summary>
    /// Provides a set of methods for working with <see cref="Type">types</see>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Returns all base types of a given type, both open and closed generic (if any), including itself.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to get for.</param>
        /// <returns>All base and implementing <see cref="Type">types</see>.</returns>
        public static IEnumerable<Type> AllBaseAndImplementingTypes(this Type type)
        {
            return type.BaseTypes()
                .Concat(type.GetTypeInfo().GetInterfaces())
                .SelectMany(ThisAndMaybeOpenType)
                .Where(t => t != type && t != typeof(object));
        }

        static IEnumerable<Type> BaseTypes(this Type type)
        {
            var currentType = type;
            while (currentType != null)
            {
                yield return currentType;
                currentType = currentType.GetTypeInfo().BaseType;
            }
        }

        static IEnumerable<Type> ThisAndMaybeOpenType(Type type)
        {
            yield return type;
            if (type.GetTypeInfo().IsGenericType && !type.GetTypeInfo().ContainsGenericParameters)
            {
                yield return type.GetGenericTypeDefinition();
            }
        }
    }
}
