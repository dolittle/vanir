// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Concepts;
using Dolittle.Vanir.Backend.Reflection;
using Dolittle.Vanir.Backend.Strings;
using FluentValidation;
using FluentValidation.Results;
using HotChocolate;
using HotChocolate.Resolvers;

namespace Dolittle.Vanir.Backend.GraphQL.Validation
{
    public class ValidationMiddleware
    {
        readonly FieldDelegate _next;
        readonly IValidators _validators;

        public ValidationMiddleware(FieldDelegate next, IValidators validators)
        {
            _next = next;
            _validators = validators;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            foreach (var argument in context.Field.Arguments)
            {
                if (_validators.HasFor(argument.RuntimeType))
                {
                    var instance = context.CallGenericMethod<object, IMiddlewareContext, NameString>(_ => _.ArgumentValue<object>, argument.Name, argument.RuntimeType);
                    var errors = new List<IError>();
                    await CheckAndValidate(context, instance, argument.RuntimeType, errors, argument.Name);
                    if (errors.Count > 0)
                    {
                        context.Result = errors;
                        return;
                    }
                }
            }

            await _next(context);
        }

        async Task CheckAndValidate(IMiddlewareContext context, object instance, Type type, List<IError> errors, string propertyPath)
        {
            if (_validators.HasFor(type))
            {
                var validator = _validators.GetFor(type);
                var validationContextType = typeof(ValidationContext<>).MakeGenericType(type);
                var validationContext = Activator.CreateInstance(validationContextType, instance) as IValidationContext;
                var result = await validator.ValidateAsync(validationContext);
                if (!result.IsValid)
                {
                    CollectErrors(context, result, errors, type, propertyPath);
                }
            }

            if (!type.IsAPrimitiveType())
            {
                foreach (var property in type.GetProperties())
                {
                    var propertyInstance = property.GetValue(instance);
                    if (propertyInstance != null)
                    {
                        var localPropertyPath = $"{propertyPath}.{property.Name.ToCamelCase()}";
                        await CheckAndValidate(context, propertyInstance, property.PropertyType, errors, localPropertyPath);
                    }
                }
            }
        }


        void CollectErrors(IMiddlewareContext context, ValidationResult result, List<IError> errors, Type type, string propertyPath)
        {
            foreach (var validationError in result.Errors)
            {
                var propertyName = type.IsConcept() ? propertyPath : $"{propertyPath}.{validationError.PropertyName.ToCamelCase()}";
                errors.Add(ErrorBuilder.New()
                .SetMessage(validationError.ErrorCode)
                .SetExtension(propertyName, validationError.ErrorMessage)
                .SetPath(context.Path)
                .Build());
            }
        }
    }
}
