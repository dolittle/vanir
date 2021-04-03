// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Reflection;
using FluentValidation;
using FluentValidation.Results;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;

namespace Dolittle.Vanir.Backend.GraphQL.Validation
{
    public class ValidationMiddleware
    {
        readonly FieldDelegate _next;
        readonly IValidators _validators;
        readonly INamingConventions _namingConventions;

        public ValidationMiddleware(FieldDelegate next, IValidators validators, INamingConventions namingConventions)
        {
            _next = next;
            _validators = validators;
            _namingConventions = namingConventions;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            foreach (var argument in context.Field.Arguments)
            {
                if (_validators.HasFor(argument.RuntimeType))
                {
                    var instance = context.CallGenericMethod<object, IMiddlewareContext, NameString>(_ => _.ArgumentValue<object>, argument.Name, argument.RuntimeType);
                    var errors = new List<IError>();
                    await CheckAndValidate(context, instance, argument.RuntimeType, errors);
                    if (errors.Count > 0)
                    {
                        context.Result = errors;
                        return;
                    }
                }
            }

            await _next(context);
        }

        async Task CheckAndValidate(IMiddlewareContext context, object instance, Type type, List<IError> errors)
        {
            var validator = _validators.GetFor(type);
            var validationContextType = typeof(ValidationContext<>).MakeGenericType(type);
            var validationContext = Activator.CreateInstance(validationContextType, instance) as IValidationContext;
            var result = await validator.ValidateAsync(validationContext);
            if (!result.IsValid)
            {
                CollectErrors(context, result, errors);
            }
            foreach (var property in type.GetProperties())
            {
                if (_validators.HasFor(property.PropertyType))
                {
                    var propertyInstance = property.GetValue(instance);
                    if (propertyInstance != null)
                    {
                        await CheckAndValidate(context, propertyInstance, property.PropertyType, errors);
                    }
                }
            }
        }

        void CollectErrors(IMiddlewareContext context, ValidationResult result, List<IError> errors)
        {
            foreach (var validationError in result.Errors)
            {
                errors.Add(ErrorBuilder.New()
                .SetMessage(validationError.ErrorCode)
                .SetExtension(validationError.PropertyName, validationError.ErrorMessage)
                .SetPath(context.Path)
                .Build());
            }
        }
    }
}
