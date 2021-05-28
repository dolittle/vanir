// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Dolittle.Vanir.CLI.EventHorizon
{
    public class EventHorizonCommandProvider : ICanProvideCommand
    {
        readonly Connect _connect;

        public EventHorizonCommandProvider(Connect connect)
        {
            _connect = connect;
        }

        public Command Provide()
        {
            var list = new Command("list", "List all connections between microservices");
            return new Command("eventhorizon", "Work with event horizons")
            {
                list,
                _connect
            };
        }
    }
}
