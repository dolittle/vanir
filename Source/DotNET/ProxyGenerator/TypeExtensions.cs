// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Vanir.Backend.Concepts;

namespace Dolittle.Vanir.ProxyGenerator
{
    public static class TypeExtensions
    {
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static Type GetNullableType(this Type type)
        {
            return type.GetGenericArguments()[0];
        }

        public static bool IsComplexType(this Type type)
        {
            if (type.IsNullable()) return IsComplexType(type.GetNullableType());
            if (type.IsEnum) return false;
            if (type.IsConcept()) return false;
            if (type.IsPrimitive) return false;
            if (type == typeof(DateTime) ||
                type == typeof(DateTimeOffset) ||
                type == typeof(decimal) ||
                type == typeof(Guid) ||
                type == typeof(string))
            {
                return false;
            }

            return true;
        }

        public static string ToClientTypeString(this Type type)
        {
            var typeString = "any";
            var postFix = string.Empty;
            if (type.IsNullable())
            {
                type = type.GetNullableType();
                postFix = " | undefined";
            }

            if (type.IsConcept())
            {
                type = type.GetConceptValueType();
            }

            if (type.IsEnum) return "number";

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    typeString = "number";
                    break;
                case TypeCode.String:
                    typeString = "string";
                    break;
                case TypeCode.Boolean:
                    typeString = "boolean";
                    break;
                case TypeCode.DateTime:
                    typeString = "Date";
                    break;
            }

            if (type == typeof(Guid))
            {
                typeString = "string";
            }
            if (type == typeof(DateTimeOffset))
            {
                typeString = "Date";
            }

            return $"{typeString}{postFix}";
        }
    }
}
