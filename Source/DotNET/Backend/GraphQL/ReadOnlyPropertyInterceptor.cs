// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dolittle.Vanir.Backend.Reflection;
using FluentValidation;
using HotChocolate.Configuration;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Represents a <see cref="TypeInterceptor"/> that is capable of adding readonly properties as fields during schema creation.
    /// </summary>
    public class ReadOnlyPropertyInterceptor : TypeInterceptor
    {
        static readonly MethodInfo _parentMethod;

        static ReadOnlyPropertyInterceptor()
        {
            Expression<Func<IResolverContext, object>> expression = (IResolverContext ctx) => ctx.Parent<object>();
            _parentMethod = expression.GetMethodInfo().GetGenericMethodDefinition();
        }

        /// <inheritdoc/>
        public override bool CanHandle(ITypeSystemObjectContext context)
        {
            var contextType = context.Type.GetType();
            if (!contextType.IsGenericType || contextType.GetGenericTypeDefinition() != typeof(ObjectType<>))
            {
                return false;
            }

            var type = contextType.GetGenericArguments()[0];
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Any(_ => !_.CanWrite);
        }

        /// <inheritdoc/>
        public override void OnBeforeCompleteType(ITypeCompletionContext completionContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
            if (definition is ObjectTypeDefinition otd)
            {
                var parentMethodForType = _parentMethod.MakeGenericMethod(otd.RuntimeType);
                var properties = otd.RuntimeType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Where(_ => !_.CanWrite);
                foreach (var property in properties)
                {
                    var descriptor = ObjectFieldDescriptor.New(completionContext.DescriptorContext, property, otd.RuntimeType);
                    descriptor.Resolve((ctx) =>
                    {
                        var parent = parentMethodForType.Invoke(ctx, Array.Empty<object>());
                        return property.GetValue(parent);
                    });
                    var propertyDefinition = descriptor.CreateDefinition();
                    otd.Fields.Add(propertyDefinition);
                }
            }
        }
    }
}
