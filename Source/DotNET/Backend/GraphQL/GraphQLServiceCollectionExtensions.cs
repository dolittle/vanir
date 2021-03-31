// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.SDK.Concepts;
using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.GraphQL.Concepts;
using Dolittle.Vanir.Backend.Reflection;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> for adding GraphQL services.
    /// </summary>
    public static class GraphQLServiceCollectionExtensions
    {
        public const char UuidFormat = 'D';

        /// <summary>
        /// Add GraphQL services.
        /// </summary>
        /// <param name="services"><see cref="IServiceColletion"/> to add to.</param>
        public static void AddGraphQL(this IServiceCollection services, BackendArguments arguments = null, ITypes types = null)
        {
            if (types == null)
            {
                types = new Types();
                services.Add(new ServiceDescriptor(typeof(ITypes), types));
            }

            var graphControllers = new GraphControllers(types);
            services.Add(new ServiceDescriptor(typeof(IGraphControllers), graphControllers));

            foreach (var graphControllerType in graphControllers.All)
            {
                services.Add(new ServiceDescriptor(graphControllerType, graphControllerType, ServiceLifetime.Transient));
            }

            var graphQLBuilder = services
                                    .AddGraphQLServer()
                                    .AddType(new UuidType(UuidFormat));
            types.FindMultiple<ScalarType>().Where(_ => !_.IsGenericType).ForEach(_ => graphQLBuilder.AddType(_));

            graphQLBuilder
                .AddQueryType<QueryType>()
                .AddMutationType<MutationType>();

            types.FindMultiple(typeof(ConceptAs<>)).ForEach(_ => graphQLBuilder.AddConceptTypeConverter(_));

            services.AddSingleton<INamingConventions, NamingConventions>();

            arguments?.GraphQLExecutorBuilder(graphQLBuilder);

            if (RuntimeEnvironment.isDevelopment)
            {
                graphQLBuilder.AddApolloTracing();
            }
        }
    }

}
