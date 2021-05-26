// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Dolittle.Vanir.CLI.Microservices
{
    public class MicroserviceCommandProvider : ICanProvideCommand
    {
        public Command Provide()
        {
            return new Command("microservice", "Work with a single microservice")
            {
                GetProducersCommands(),
                GetConsumersCommands()
            };
        }

        Command GetConsumersCommands()
        {
            var consumersCommand = new Command("consumers", "Work with configuration related to microservices that consume events from your microservice.");
            var listConsumersCommand = new Command("list", "List all configured consumers");
            consumersCommand.Add(listConsumersCommand);
            return consumersCommand;
        }

        Command GetProducersCommands()
        {
            var producersCommand = new Command("producers", "Work with configuration related to microservices that produce events that we are interested in.");

            var listProducersCommand = new Command("list", "List all configured producers");
            producersCommand.Add(listProducersCommand);
            return producersCommand;
        }
    }
}
