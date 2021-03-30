// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.Reflection;
using HotChocolate.Types;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> for adding GraphQL services.
    /// </summary>
    public static class GraphQLServiceCollectionExtensions
    {
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
                                    .AddType(new UuidType('D'))
                                    .AddQueryType<QueryType>()
                                    .AddMutationType<MutationType>();

            arguments?.GraphQLExecutorBuilder(graphQLBuilder);

            types.FindMultiple<ScalarType>().ForEach(_ => graphQLBuilder.AddType(_));

            if (RuntimeEnvironment.isDevelopment)
            {
                graphQLBuilder.AddApolloTracing();
            }
        }
    }
}
