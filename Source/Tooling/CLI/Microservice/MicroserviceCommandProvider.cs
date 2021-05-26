// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Dolittle.Vanir.CLI.Microservice
{
    public class MicroserviceCommandProvider : ICanProvideCommand
    {
        readonly ListConsumers _listConsumers;

        public MicroserviceCommandProvider(ListConsumers listConsumers)
        {
            _listConsumers = listConsumers;
        }

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
            var listConsumersCommand = new Command("list", "List all configured consumers")
            {
                Handler = _listConsumers
            };
            consumersCommand.Add(listConsumersCommand);

            var addConsumer = new Command("add", "Add a producer we want events from")
            {
                new Argument<string>("microservice", "The id or name of the microservice to add.")
            };
            consumersCommand.Add(addConsumer);

            return consumersCommand;
        }

        Command GetProducersCommands()
        {
            var producersCommand = new Command("producers", "Work with configuration related to microservices that produce events that we are interested in.");

            var listProducersCommand = new Command("list", "List all configured producers");
            producersCommand.Add(listProducersCommand);

            var addProducerCommand = new Command("add", "Add a producer we want events from")
            {
                new Argument<string>("microservice", "The id or name of the microservice to add.")
            };
            producersCommand.Add(addProducerCommand);

            return producersCommand;
        }
    }
}
