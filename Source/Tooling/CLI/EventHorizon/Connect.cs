// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Dolittle;
using Dolittle.Vanir.CLI.Contexts;
using Dolittle.Vanir.CLI.Tenants;

namespace Dolittle.Vanir.CLI.EventHorizon
{
    public class Connect : Command, ICommandHandler
    {
        public static class AllArguments
        {
            public static readonly Argument<string> Producer = new("producer", "Name or Id of the producing microservice");
            public static readonly Argument<string> Consumer = new("consumer", "Name or Id of the consuming microservice");
        }

        public static class AllOptions
        {
            public static readonly Option<Guid> Tenant = new(new[] { "--tenant", "-t" }, "Tenant to add for. Default will be all.");
            public static readonly Option<Guid> ConsumeTenant = new(new[] { "--consumer-tenant", "-ct" }, "Consumer tenant to add for. Default will be using producer tenant. If producer tenant is not specified, this will be ignored.");
            public static readonly Option<Guid> Stream = new(new[] { "--stream", "-s" }, "Public stream to use. Default will be the same Id as the identifier of the producing microservice.");
            public static readonly Option<Guid> Partition = new(new[] { "--partition", "-p" }, "Partition Id to use. Default is 00000000-0000-0000-0000-000000000000.");
            public static readonly Option<Guid> Scope = new(new[] { "--scope", "-sc" }, "Scope Id to use in consumer. Default will be the same as the stream identifier.");
            public static readonly Option<bool> Overwrite = new(new[] { "--overwrite", "-o" }, (_) => true, isDefault: false, description: "Overwrite any existing configuration.");
        }

        readonly ContextOf<ApplicationContext> _getApplicationContext;

        public Connect(ContextOf<ApplicationContext> getApplicationContext)
            : base("connect", "Connect a microservice that produces public events to a consuming microservice")
        {
            AddArgument(AllArguments.Producer);
            AddArgument(AllArguments.Consumer);

            AddOption(AllOptions.Tenant);
            AddOption(AllOptions.ConsumeTenant);
            AddOption(AllOptions.Stream);
            AddOption(AllOptions.Partition);
            AddOption(AllOptions.Overwrite);
            _getApplicationContext = getApplicationContext;

            Handler = this;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var applicationContext = _getApplicationContext();
            var producer = context.ParseResult.ValueForArgument(AllArguments.Producer);
            var consumer = context.ParseResult.ValueForArgument(AllArguments.Consumer);
            var overwrite = context.ParseResult.ValueForOption(AllOptions.Overwrite);

            var producerExists = MicroserviceShouldExist(context, applicationContext, producer, out MicroserviceContext producerMicroserviceContext);
            var consumerExists = MicroserviceShouldExist(context, applicationContext, consumer, out MicroserviceContext consumerMicroserviceContext);

            if (!producerExists || !consumerExists)
            {
                return Task.FromResult(-1);
            }

            ProducerAndConsumerCannotBeTheSame(context, producerMicroserviceContext, consumerMicroserviceContext);

            EventHorizonConsents consents;
            EventHorizons eventHorizons;
            if (overwrite)
            {
                consents = new();
                eventHorizons = new();
            }
            else
            {
                consents = producerMicroserviceContext.GetEventHorizonConsents();
                eventHorizons = consumerMicroserviceContext.GetEventHorizons();
            }

            SetupConsentsForAllTenants(context, producerMicroserviceContext, consumerMicroserviceContext, consents);
            SetupEventHorizonsForAllTenants(context, producerMicroserviceContext, consumerMicroserviceContext, eventHorizons);

            producerMicroserviceContext.SaveEventHorizonConsents(consents);
            consumerMicroserviceContext.SaveEventHorizons(eventHorizons);

            return Task.FromResult(0);
        }

        void SetupConsentsForAllTenants(InvocationContext context, MicroserviceContext producerMicroserviceContext, MicroserviceContext consumerMicroserviceContext, EventHorizonConsents consentsPerTenant)
        {
            foreach (var tenant in producerMicroserviceContext.Application.GetTenants())
            {
                var consents = new List<EventHorizonConsent>();
                if (consentsPerTenant.ContainsKey(tenant))
                {
                    consents.AddRange(consentsPerTenant[tenant]);
                }

                var microserviceId = Guid.Parse(consumerMicroserviceContext.Microservice.Id);
                var streamId = Guid.Parse(producerMicroserviceContext.Microservice.Id);
                var partitionId = Guid.Empty;

                var consent = new EventHorizonConsent()
                {
                    Microservice = microserviceId,
                    Tenant = tenant,
                    Stream = streamId,
                    Partition = partitionId,
                    Consent = Guid.NewGuid()
                };

                DuplicateConsentsNotAllowed(context, producerMicroserviceContext, consumerMicroserviceContext, consents, consent);

                consents.Add(consent);
                consentsPerTenant[tenant] = consents.ToArray();
            }
        }

        void SetupEventHorizonsForAllTenants(InvocationContext context, MicroserviceContext producerMicroserviceContext, MicroserviceContext consumerMicroserviceContext, EventHorizons eventHorizonsPerTenant)
        {
            foreach (var tenant in producerMicroserviceContext.Application.GetTenants())
            {
                var eventHorizons = new List<EventHorizonDefinition>();
                if (eventHorizonsPerTenant.ContainsKey(tenant))
                {
                    eventHorizons.AddRange(eventHorizonsPerTenant[tenant]);
                }

                var microserviceId = Guid.Parse(producerMicroserviceContext.Microservice.Id);
                var streamId = microserviceId;
                var partitionId = Guid.Empty;
                var scopeId = streamId;

                var eventHorizon = new EventHorizonDefinition()
                {
                    Microservice = microserviceId,
                    Tenant = tenant,
                    Stream = streamId,
                    Partition = partitionId,
                    Scope = scopeId
                };

                DuplicateEventHorizonsNotAllowed(context, producerMicroserviceContext, consumerMicroserviceContext, eventHorizons, eventHorizon);

                eventHorizons.Add(eventHorizon);
                eventHorizonsPerTenant[tenant] = eventHorizons.ToArray();
            }
        }

        bool MicroserviceShouldExist(InvocationContext context, ApplicationContext applicationContext, string microservice, out MicroserviceContext microserviceContext)
        {
            var microserviceId = Guid.Empty;
            microserviceContext = null;
            if (Guid.TryParse(microservice, out microserviceId))
            {
                microserviceContext = applicationContext.Application.Microservices.SingleOrDefault(_ => Guid.Parse(_.Microservice.Id) == microserviceId);
            }
            else
            {
                microserviceContext = applicationContext.Application.Microservices.SingleOrDefault(_ => _.Microservice.Name == microservice);
            }

            if (microserviceContext == null)
            {
                context.Console.Error.Write($"There is no microservice with name/id '{microservice}'");
                Environment.Exit(-1);
                return false;
            }

            return true;
        }

        void ProducerAndConsumerCannotBeTheSame(InvocationContext context, MicroserviceContext producer, MicroserviceContext consumer)
        {
            if (producer.Microservice.Id.Equals(consumer.Microservice.Id, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Console.Error.Write($"Can't connect: Producer '{producer.Microservice.Name}' has the same identifier as consumer '{consumer.Microservice.Name}'");
                Environment.Exit(-1);
            }
        }

        void DuplicateConsentsNotAllowed(InvocationContext context, MicroserviceContext producer, MicroserviceContext consumer, IEnumerable<EventHorizonConsent> consents, EventHorizonConsent consent)
        {
            if (consents.Any(_ =>
                _.Microservice == consent.Microservice &&
                _.Tenant == consent.Tenant &&
                _.Stream == consent.Stream &&
                _.Partition == consent.Partition))
            {
                context.Console.Error.Write($"There already is a consent for '{consumer.Microservice.Name} ({consumer.Microservice.Id}' registered in producer '{producer.Microservice.Name} ({producer.Microservice.Id})'");
                Environment.Exit(-1);
            }
        }

        void DuplicateEventHorizonsNotAllowed(InvocationContext context, MicroserviceContext producer, MicroserviceContext consumer, IEnumerable<EventHorizonDefinition> eventHorizons, EventHorizonDefinition eventHorizon)
        {
            if (eventHorizons.Any(_ =>
                _.Microservice == eventHorizon.Microservice &&
                _.Tenant == eventHorizon.Tenant &&
                _.Stream == eventHorizon.Stream &&
                _.Partition == eventHorizon.Partition &&
                _.Scope == eventHorizon.Scope))
            {
                context.Console.Error.Write($"There already is an event horizon for '{consumer.Microservice.Name} ({consumer.Microservice.Id}' registered in producer '{producer.Microservice.Name} ({producer.Microservice.Id})'");
                Environment.Exit(-1);
            }
        }
    }
}
