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
        static readonly HashSet<Type> _additionalPrimitiveTypes = new()
        {
            typeof(decimal),
            typeof(string),
            typeof(Guid),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan)
        };

        static readonly HashSet<Type> _numericTypes = new()
        {
            typeof(byte),
            typeof(sbyte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(double),
            typeof(decimal),
            typeof(float)
        };

        /// <summary>
        /// Check if a type derives from an open generic type.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <param name="openGenericType">Open generic <see cref="Type"/> to check for.</param>
        /// <returns>True if type matches the open generic <see cref="Type"/>.</returns>
        public static bool IsDerivedFromOpenGeneric(this Type type, Type openGenericType)
        {
            var typeToCheck = type;
            while (typeToCheck != null && typeToCheck != typeof(object))
            {
                var currentType = typeToCheck.GetTypeInfo().IsGenericType ? typeToCheck.GetGenericTypeDefinition() : typeToCheck;
                if (openGenericType == currentType)
                    return true;

                typeToCheck = typeToCheck.GetTypeInfo().BaseType;
            }

            return false;
        }

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

        /// <summary>
        /// Check if a type is nullable or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is nullable, false if not.</returns>
        public static bool IsNullable(this Type type)
        {
            return type.GetTypeInfo().IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Check if a type is a number or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is numeric, false if not.</returns>
        public static bool IsNumericType(this Type type)
        {
            return _numericTypes.Contains(type) ||
                   _numericTypes.Contains(Nullable.GetUnderlyingType(type));
        }

        /// <summary>
        /// Check if a type is enumerable. Note that string is an IEnumerable, but in this case the string is excluded.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is enumerable, false if not an enumerable.</returns>
        public static bool IsEnumerable(this Type type)
        {
            return !type.IsAPrimitiveType() && !type.IsString() && typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// Check if the type is of the type specified in the generic param.
        /// </summary>
        /// <typeparam name="T">Type of the instance.</typeparam>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is a date, false if not.</returns>
        public static bool Is<T>(this Type type)
        {
            return type == typeof(T) || Nullable.GetUnderlyingType(type) == typeof(T);
        }

        /// <summary>
        /// Check if a type is a Date or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is a date, false if not.</returns>
        public static bool IsDate(this Type type)
        {
            return Is<DateTime>(type);
        }

        /// <summary>
        /// Check if a type is a DateTimeOffset or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is a date, false if not.</returns>
        public static bool IsDateTimeOffset(this Type type)
        {
            return Is<DateTimeOffset>(type);
        }

        /// <summary>
        /// Check if a type is a Boolean or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is a boolean, false if not.</returns>
        public static bool IsBoolean(this Type type)
        {
            return Is<bool>(type);
        }

        /// <summary>
        /// Check if a type is a String or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is a string, false otherwise.</returns>
        public static bool IsString(this Type type)
        {
            return Is<string>(type);
        }

        /// <summary>
        /// Check if a type is a Guid or not.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if type is a Guid, false otherwise.</returns>
        public static bool IsGuid(this Type type)
        {
            return Is<Guid>(type);
        }

        /// <summary>
        /// Check if a type is a "primitve" type.  This is not just dot net primitives but basic types like string, decimal, datetime,
        /// that are not classified as primitive types.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to check.</param>
        /// <returns>True if <see cref="Type"/> is a primitive type.</returns>
        public static bool IsAPrimitiveType(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive
                    || type.IsNullable() || _additionalPrimitiveTypes.Contains(type);
        }

        /// <summary>
        /// Get <see cref="ITypeInfo"/> from <see cref="Type"/>.
        /// </summary>
        /// <param name="type"><see cref="Type"/> to get from.</param>
        /// <returns>The <see cref="ITypeInfo"/>.</returns>
        internal static ITypeInfo GetTypeInfo(Type type)
        {
            var typeInfoType = typeof(TypeInfo<>).MakeGenericType(type);
            var instanceField = typeInfoType.GetTypeInfo().GetField("Instance", BindingFlags.Public | BindingFlags.Static);
            return instanceField.GetValue(null) as ITypeInfo;
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
