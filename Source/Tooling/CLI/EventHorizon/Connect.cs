// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
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
            public static readonly Option<Guid> Tenant = new("--tenant", "Tenant to add for. Default will be all.");
            public static readonly Option<Guid> ConsumeTenant = new("--consumer-tenant", "Consumer tenant to add for. Default will be using producer tenant. If producer tenant is not specified, this will be ignored.");
            public static readonly Option<Guid> Stream = new("--stream", "Public stream to use. Default will be the same Id as the identifier of the producing microservice");
            public static readonly Option<Guid> Partition = new("--partition", "Partition Id to use. Default is 00000000-0000-0000-0000-000000000000");
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
            _getApplicationContext = getApplicationContext;

            Handler = this;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var applicationContext = _getApplicationContext();
            var producer = context.ParseResult.ValueForArgument(AllArguments.Producer);
            var consumer = context.ParseResult.ValueForArgument(AllArguments.Consumer);

            var producerExists = MicroserviceShouldExist(context, applicationContext, producer, out MicroserviceContext producerMicroserviceContext);
            var consumerExists = MicroserviceShouldExist(context, applicationContext, consumer, out MicroserviceContext consumerMicroserviceContext);

            if (!producerExists || !consumerExists)
            {
                return Task.FromResult(-1);
            }

            ProducerAndConsumerCannotBeTheSame(context, producerMicroserviceContext, consumerMicroserviceContext);

            /*
             Default - no options specified:
             loop through all tenants of producer:
             - Check if consent is already there - if so exit with message
             - Add consent
                - Default stream = Microservice Id

             For consumer:
             - Add consumer configuration
            */

            var tenants = producerMicroserviceContext.GetTenants();
            var consentsPerTenant = producerMicroserviceContext.GetEventHorizonConsents();

            foreach (var tenant in tenants)
            {
                var consents = new List<EventHorizonConsent>();
                if (consentsPerTenant.ContainsKey(tenant))
                {
                    consents.AddRange(consentsPerTenant[tenant]);
                }

                var microserviceId = Guid.Parse(consumerMicroserviceContext.Microservice.Id);

                var consent = new EventHorizonConsent()
                {
                    Microservice = microserviceId,
                    Tenant = tenant,
                    Stream = microserviceId,
                    Partition = Guid.Empty,
                    Consent = Guid.NewGuid()
                };

                DuplicateConsentsNotAllowed(context, producerMicroserviceContext, consumerMicroserviceContext, consents, consent);

                consents.Add(consent);
                consentsPerTenant[tenant] = consents.ToArray();
            }

            producerMicroserviceContext.SaveEventHorizonConsents(consentsPerTenant);

            return Task.FromResult(0);
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
    }
}
