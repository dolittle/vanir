// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.SDK.Concepts;
using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.Dolittle;
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.GraphQL.Concepts;
using Dolittle.Vanir.Backend.GraphQL.Validation;
using Dolittle.Vanir.Backend.Reflection;
using FluentValidation;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> for adding GraphQL services.
    /// </summary>
    public static partial class GraphQLServiceCollectionExtensions
    {
        public const char UuidFormat = 'D';

        /// <summary>
        /// Add GraphQL services.
        /// </summary>
        /// <param name="services"><see cref="IServiceColletion"/> to add to.</param>
        public static void AddGraphQL(this IServiceCollection services, IContainer container, BackendArguments arguments = null, ITypes types = null)
        {
            if (types == null)
            {
                types = new Types();
                services.Add(new ServiceDescriptor(typeof(ITypes), types));
            }

            services.AddFluentValidation(container, types);

            var graphControllers = new GraphControllers(types);
            services.Add(new ServiceDescriptor(typeof(IGraphControllers), graphControllers));

            services.AddSingleton<ITypeInspector, TypeInspector>();

            foreach (var graphControllerType in graphControllers.All)
            {
                services.Add(new ServiceDescriptor(graphControllerType, graphControllerType, ServiceLifetime.Transient));
            }

            var graphQLBuilder = services
                                    .AddGraphQLServer()
                                    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = RuntimeEnvironment.isDevelopment)
                                    .TryAddTypeInterceptor<ReadOnlyPropertyInterceptor>()
                                    .AddAuthorization()
                                    .UseFluentValidation()
                                    .AddType(new UuidType(UuidFormat));
            types.FindMultiple<ScalarType>().Where(_ => !_.IsGenericType).ForEach(_ => graphQLBuilder.AddType(_));

            var namingConventions = new NamingConventions();

            graphQLBuilder
                .AddQueries(graphControllers, namingConventions, out SchemaRoute queries)
                .AddMutations(graphControllers, namingConventions, out SchemaRoute mutations);

            types.FindMultiple(typeof(ConceptAs<>)).ForEach(_ => graphQLBuilder.AddConceptTypeConverter(_));
            services.AddSingleton<INamingConventions>(namingConventions);

            arguments?.GraphQLExecutorBuilder(graphQLBuilder);

            if (RuntimeEnvironment.isDevelopment)
            {
                if (arguments.ExposeEventsInGraphQLSchema) mutations.AddEventsAsMutations(types);
                graphQLBuilder.AddApolloTracing();
            }

            var scannedTypes = new Dictionary<Type, Type>();
            graphQLBuilder.RegisterAllEnumsFor(queries, scannedTypes);
            graphQLBuilder.RegisterAllEnumsFor(mutations, scannedTypes);
        }
    }
}
