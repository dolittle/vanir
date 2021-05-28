// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Invocation;

namespace Dolittle.Vanir.CLI.Microservices
{

    public class MicroservicesCommandProvider : ICanProvideCommand
    {
        readonly ListMicroservices _listMicroservices;

        public MicroservicesCommandProvider(ListMicroservices listMicroservices)
        {
            _listMicroservices = listMicroservices;
        }

        public Command Provide()
        {
            var command = new Command("microservices", "Work with microservices configuration");

            var listCommand = new Command("list", "List microservices within your application")
            {
                Handler = _listMicroservices
            };
            command.Add(listCommand);

            var addCommand = new Command("add");
            command.Add(addCommand);

            var removeCommand = new Command("remove");
            command.Add(removeCommand);

            return command;
        }
    }
}
