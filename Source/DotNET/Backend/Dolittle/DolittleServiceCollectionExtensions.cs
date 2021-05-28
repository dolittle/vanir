// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.SDK;
using Dolittle.SDK.Aggregates;
using Dolittle.SDK.Events;
using Dolittle.SDK.Events.Builders;
using Dolittle.SDK.Events.Filters;
using Dolittle.SDK.Events.Handling;
using Dolittle.SDK.Events.Handling.Builder;
using Dolittle.SDK.Projections;
using Dolittle.SDK.Projections.Builder;
using Dolittle.Vanir.Backend;
using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.Dolittle;
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
                .WithEventHorizons(_ =>
                {
                    var eventHorizons = EventHorizons.Load();
                    foreach (var tenant in eventHorizons.Keys)
                    {
                        _.ForTenant(tenant, sb =>
                        {
                            foreach (var subscription in eventHorizons[tenant])
                            {
                                sb
                                    .FromProducerMicroservice(subscription.Microservice)
                                    .FromProducerTenant(subscription.Tenant)
                                    .FromProducerStream(subscription.Stream)
                                    .FromProducerPartition(subscription.Partition)
                                    .ToScope(subscription.Scope);
                            }
                        });
                    }
                })
                .WithFilters(_ =>
                {
                    if (arguments?.PublishAllPublicEvents == true)
                    {
                        _.CreatePublicFilter(configuration.MicroserviceId, _ => _
                            .Handle((e, ec) => Task.FromResult(new PartitionedFilterResult(true, PartitionId.Unspecified))));
                    }

                    AddDevelopmentFilters(_);
                })
                .WithProjections(_ => AllProjections(_, types))
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
                return DolittleClient.EventStore.ForTenant(ExecutionContextManager.GetCurrent().Tenant);
            });
            services.AddTransient(typeof(IAggregateOf<>), typeof(AggregateOf<>));

            services.AddSingleton<IEventTypes>(_ =>
            {
                ThrowIfClientNotBuilt();
                return DolittleClient.EventTypes;
            });

            services.AddExecutionContext();

            DolittleClientBuilder = clientBuilder;
        }

        static void ConfigureEventHorizons(Dolittle.SDK.EventHorizon.SubscriptionsBuilder subscriptionsBuilder)
        {
            var eventHorizons = EventHorizons.Load();
            foreach (var tenant in eventHorizons.Keys)
            {
                subscriptionsBuilder.ForTenant(tenant, tb =>
                {
                    foreach (var subscription in eventHorizons[tenant])
                    {
                        tb
                            .FromProducerMicroservice(subscription.Microservice)
                            .FromProducerTenant(subscription.Tenant)
                            .FromProducerStream(subscription.Stream)
                            .FromProducerPartition(subscription.Partition)
                            .ToScope(subscription.Scope);
                    }
                });
            }
        }

        static void AddDevelopmentFilters(EventFiltersBuilder eventFiltersBuilder)
        {
            if (RuntimeEnvironment.IsDevelopment)
            {
                var eventHorizons = EventHorizons.Load();
                foreach (var tenant in eventHorizons.Keys)
                {
                    foreach (var subscription in eventHorizons[tenant])
                    {
                        eventFiltersBuilder.CreatePrivateFilter("f099003a-a37c-4106-9917-5ebe59bb908e", fb =>
                        {
                            fb.InScope(subscription.Scope).Unpartitioned().Handle(async (e, ec) =>
                            {
                                await EventStreamSubscription.EventHandler(e, ec);
                                return true;
                            });
                        });
                    }
                }
            }
        }

        static void AllEventTypes(EventTypesBuilder builder, ITypes types)
        {
            types.All
                    .Where(_ => _.HasAttribute<EventTypeAttribute>())
                    .ForEach(_ => builder.Register(_));
        }

        static void AllProjections(ProjectionsBuilder builder, ITypes types)
        {
            types.All
                    .Where(_ => _.HasAttribute<ProjectionAttribute>())
                    .ForEach(_ => builder.RegisterProjection(_));
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
