// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.Vanir.Backend.Concepts;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using HotChocolate.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Dolittle.Vanir.Backend.GraphQL.Concepts
{
    public static class ConceptAsGraphQLBuilderExtensions
    {
        public static IRequestExecutorBuilder AddConceptTypeConverter(this IRequestExecutorBuilder builder, Type type)
        {
            var conceptValueType = type.GetConceptValueType();

            if (conceptValueType == typeof(bool))
            {
                builder.BindRuntimeType(type, typeof(BooleanType));
            }
            else if (conceptValueType == typeof(decimal))
            {
                builder.BindRuntimeType(type, typeof(DecimalType));
            }
            else if (conceptValueType == typeof(double))
            {
                builder.BindRuntimeType(type, typeof(FloatType));
            }
            else if (conceptValueType == typeof(float))
            {
                builder.BindRuntimeType(type, typeof(FloatType));
            }
            else if (conceptValueType == typeof(Guid))
            {
                builder.BindRuntimeType(type, typeof(UuidType));
            }
            else if (conceptValueType == typeof(int))
            {
                builder.BindRuntimeType(type, typeof(IntType));
            }
            else if (conceptValueType == typeof(long))
            {
                builder.BindRuntimeType(type, typeof(IntType));
            }
            else if (conceptValueType == typeof(string))
            {
                builder.BindRuntimeType(type, typeof(StringType));
            }

            var provider = Activator.CreateInstance(typeof(ConceptAsTypeProvider<>).MakeGenericType(type));
            builder.Services.AddSingleton(provider as IChangeTypeProvider);

            return builder;
        }
    }
}
