// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine;

namespace Dolittle.Vanir.CLI.EventHorizon
{
    public class EventHorizonCommandProvider : ICanProvideCommand
    {

        public EventHorizonCommandProvider()
        {
        }

        public Command Provide()
        {
            var list = new Command("list", "List all connections between microservices");

            var connect = new Command("connect", "Connect a microservice that produces public events to a consuming microservice")
                {
                    new Argument<string>("producer", "Name or Id of the producing microservice"),
                    new Argument<string>("consume", "Name or Id of the consuming microservice")
                };

            connect.AddOption(new Option<Guid>("--tenant", "Tenant to add for. Default will be all."));
            connect.AddOption(new Option<Guid>("--consumer-tenant", "Consumer tenant to add for. Default will be using producer tenant. If producer tenant is not specified, this will be ignored."));
            connect.AddOption(new Option<Guid>("--stream", "Public stream to use. Default will be the same Id as the identifier of the producing microservice"));
            connect.AddOption(new Option<Guid>("--partition", "Partition Id to use. Default is 00000000-0000-0000-0000-000000000000"));

            return new Command("eventhorizon", "Work with event horizons")
            {
                list,
                connect
            };
        }
    }
}
