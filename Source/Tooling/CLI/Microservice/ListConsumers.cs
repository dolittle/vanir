// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.Microservice
{
    public class ListConsumers : ICommandHandler
    {
        readonly ContextOf<MicroserviceContext> _getMicroserviceContext;

        public ListConsumers(ContextOf<MicroserviceContext> getMicroserviceContext)
        {
            _getMicroserviceContext = getMicroserviceContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var microserviceContext = _getMicroserviceContext();
            var consents = microserviceContext.GetEventHorizonConsents();
            if (consents.Count == 0)
            {
                context.Console.Out.Write("No consumer consents has been configured yet.");
                Environment.Exit(0);
                return Task.FromResult(0);
            }

            return Task.FromResult(0);
        }
    }
}
