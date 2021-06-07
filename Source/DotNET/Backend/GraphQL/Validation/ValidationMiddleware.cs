// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Concepts;
using Dolittle.Vanir.Backend.Reflection;
using Dolittle.Vanir.Backend.Strings;
using Dolittle.Vanir.Backend.Validation;
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
                    await CheckAndValidate(context, instance, argument.RuntimeType, errors, argument.Name).ConfigureAwait(false);
                    if (errors.Count > 0)
                    {
                        context.Result = errors;
                        return;
                    }
                }
            }

            await _next(context).ConfigureAwait(false);
        }

        async Task CheckAndValidate(IMiddlewareContext context, object instance, Type type, List<IError> errors, string propertyPath)
        {
            if (_validators.HasFor(type))
            {
                var validators = _validators.GetFor(type);
                var validationContextType = typeof(ValidationContext<>).MakeGenericType(type);
                var validationContext = Activator.CreateInstance(validationContextType, instance) as IValidationContext;
                foreach (var validator in validators)
                {
                    var result = await validator.ValidateAsync(validationContext).ConfigureAwait(false);
                    if (!result.IsValid)
                    {
                        CollectErrors(context, instance, result, errors, type, propertyPath);
                    }
                }
            }

            if (!type.IsAPrimitiveType())
            {
                foreach (var property in type.GetProperties())
                {
                    if (property.PropertyType != type)
                    {
                        var propertyInstance = property.GetValue(instance);
                        if (propertyInstance != null)
                        {
                            var localPropertyPath = $"{propertyPath}.{property.Name.ToCamelCase()}";
                            await CheckAndValidate(context, propertyInstance, property.PropertyType, errors, localPropertyPath).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        void CollectErrors(IMiddlewareContext context, object instance, ValidationResult result, List<IError> errors, Type type, string propertyPath)
        {
            foreach (var validationError in result.Errors)
            {
                var fullPropertyPath = GetFullPropertyPath(instance, type, propertyPath, validationError);
                if (!errors.Any(_ => _.Extensions.ContainsKey(fullPropertyPath)))
                {
                    errors.Add(
                        ErrorBuilder.New()
                            .SetMessage(validationError.ErrorCode)
                            .SetExtension(fullPropertyPath, validationError.ErrorMessage)
                            .SetPath(context.Path)
                            .Build());
                }
            }
        }

        string GetFullPropertyPath(object instance, Type type, string propertyPath, ValidationFailure validationError)
        {
            var fullPropertyPath = type.IsConcept() ? propertyPath : $"{propertyPath}.{validationError.PropertyName.ToCamelCase()}";
            if (validationError.PropertyName.IndexOf(".", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                var propertyName = validationError.PropertyName[..validationError.PropertyName.IndexOf(".", StringComparison.InvariantCultureIgnoreCase)];
                var property = instance.GetType().GetProperty(propertyName);
                if (property?.PropertyType.IsConcept() == true)
                {
                    fullPropertyPath = $"{propertyPath}.{propertyName.ToCamelCase()}";
                }
            }

            return fullPropertyPath;
        }
    }
}
