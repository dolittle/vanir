// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Dolittle.SDK;
using Dolittle.SDK.Aggregates;
using Dolittle.SDK.Events;
using Dolittle.SDK.Events.Builders;
using Dolittle.SDK.Events.Handling;
using Dolittle.SDK.Events.Handling.Builder;
using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.Execution;
using Dolittle.Vanir.Backend.Reflection;
using Config = Dolittle.Vanir.Backend.Config.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DolittleServiceCollectionExtensions
    {
        internal static Client DolittleClient;
        internal static ClientBuilder DolittleClientBuilder;

        public static void AddDolittle(this IServiceCollection services, Config configuration, BackendArguments arguments = null, ITypes types = null)
        {
            if (types == null)
            {
                types = new Types();
                services.Add(new ServiceDescriptor(typeof(ITypes), types));
            }

            var clientBuilder = Client
                .ForMicroservice(configuration.MicroserviceId)
                .WithLogging(arguments.LoggerFactory)
                .WithRuntimeOn(configuration.Dolittle.Runtime.Host, configuration.Dolittle.Runtime.Port)
                .WithEventTypes(_ => AllEventTypes(_, types))
                .WithEventHandlers(_ => AllEventHandlerTypes(_, services, types));

            arguments.DolittleClientBuilderCallback(clientBuilder);

            services.AddSingleton(_ =>
            {
                ThrowIfClientNotBuilt();
                return DolittleClient;
            });
            services.AddTransient(_ =>
            {
                ThrowIfClientNotBuilt();
                return DolittleClient.EventStore.ForTenant(ExecutionContextManager.Current.Tenant);
            });
            services.AddTransient(typeof(IAggregateOf<>), typeof(AggregateOf<>));

            services.AddSingleton<IEventTypes>(_ =>
            {
                ThrowIfClientNotBuilt();
                return DolittleClient.EventTypes;
            });

            DolittleClientBuilder = clientBuilder;
        }

        static void AllEventTypes(EventTypesBuilder builder, ITypes types)
        {
            types.All
                    .Where(_ => _.HasAttribute<EventTypeAttribute>())
                    .ForEach(_ => builder.Register(_));
        }

        static void AllEventHandlerTypes(EventHandlersBuilder builder, IServiceCollection services, ITypes types)
        {
            types.All
                    .Where(_ => _.HasAttribute<EventHandlerAttribute>())
                    .ForEach(_ =>
                    {
                        builder.RegisterEventHandler(_);
                        services.AddTransient(_);
                    });
        }

        static void ThrowIfClientNotBuilt()
        {
            if (DolittleClient == null)
            {
                throw new ArgumentException("You need to call UseDolittle() or UseVanir().");
            }
        }
    }
}