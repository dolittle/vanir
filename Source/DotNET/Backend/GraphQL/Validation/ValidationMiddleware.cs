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
                    var validator = _validators.GetFor(argument.RuntimeType);
                    var instance = context.CallGenericMethod<object, IMiddlewareContext, NameString>(_ => _.ArgumentValue<object>, argument.Name, argument.RuntimeType);
                    var validationContextType = typeof(ValidationContext<>).MakeGenericType(argument.RuntimeType);
                    var validationContext = Activator.CreateInstance(validationContextType, instance) as IValidationContext;
                    var result = await validator.ValidateAsync(validationContext);
                    if (!result.IsValid)
                    {
                        SetResult(context, result);
                        return;
                    }
                }
            }

            await _next(context);
        }

        void SetResult(IMiddlewareContext context, ValidationResult result)
        {
            List<IError> errors = new();
            foreach (var validationError in result.Errors)
            {
                errors.Add(ErrorBuilder.New()
                .SetMessage(validationError.ErrorCode)
                .SetExtension(validationError.PropertyName, validationError.ErrorMessage)
                .SetPath(context.Path)
                .Build());
            }
            context.Result = errors;
        }
    }
}
