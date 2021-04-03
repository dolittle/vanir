// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.Reflection;
using FluentValidation;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types.Descriptors;
using Microsoft.Extensions.DependencyInjection;

namespace Dolittle.Vanir.Backend.GraphQL.Validation
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddFluentValidation(this IServiceCollection services, IContainer container, ITypes types)
        {
            var validators = new Validators(types, container);
            services.AddSingleton<IValidators>(validators);
            validators.All.ForEach(_ => services.AddTransient(_));
            return services;
        }

        public static IRequestExecutorBuilder UseFluentValidation(this IRequestExecutorBuilder builder)
        {
            builder.UseField<ValidationMiddleware>();
            return builder;
        }
    }
}
