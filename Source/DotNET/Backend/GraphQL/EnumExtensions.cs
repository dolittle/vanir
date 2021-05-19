// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Collections;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Dolittle.Vanir.Backend.GraphQL
{
    public static class EnumExtensions
    {
        public static void RegisterAllEnumsFor(this IRequestExecutorBuilder builder, SchemaRoute route, Dictionary<Type, Type> scannedTypes = null)
        {
            if (scannedTypes == null) scannedTypes = new();

            var types = new List<Type>();
            foreach (var item in route.Items)
            {
                var parameterTypes = item.Method.GetParameters()
                                            .Where(_ => !_.ParameterType.IsPrimitive)
                                            .Select(_ => _.ParameterType);
                types.AddRange(parameterTypes.Where(_ => !types.Contains(_)));

                var returnType = item.Method.ReturnType;
                while (returnType.IsGenericType)
                {
                    returnType = returnType.GetGenericArguments()[0];
                }

                if (!types.Contains(returnType)) types.Add(returnType);
            }

            var typesToScan = types.Where(_ => !scannedTypes.Keys.Contains(_) && !_.Namespace.StartsWith("System"));

            void TypeScanner(Type type)
            {
                var enumTypes = type.GetProperties()
                                    .Where(_ => _.PropertyType.IsEnum)
                                    .Select(_ => _.PropertyType);

                enumTypes.ForEach(_ => builder.RegisterEnumType(_));

                var complexProperties = type.GetProperties()
                                            .Where(_ => !_.PropertyType.IsPrimitive)
                                            .Select(_ => _.PropertyType);
                complexProperties.ForEach(TypeScanner);

                scannedTypes[type] = type;
            }

            typesToScan.ForEach(TypeScanner);

            foreach (var child in route.Children)
            {
                RegisterAllEnumsFor(builder, child, scannedTypes);
            }
        }

        public static void RegisterEnumType(this IRequestExecutorBuilder builder, Type type)
        {
            builder.BindRuntimeType(type, typeof(IntType));
            builder.AddTypeConverter(_ => new EnumToIntConverter(type));
        }
    }
}
