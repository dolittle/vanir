// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine;
using Dolittle.Vanir.Backend.Collections;

namespace Dolittle.Vanir.CLI
{
    public class CommandProviders
    {
        readonly IEnumerable<ICanProvideCommand> _providers;

        public CommandProviders(IEnumerable<ICanProvideCommand> providers)
        {
            _providers = providers;
        }

        public void AddAllCommandsFromProvidersTo(RootCommand command)
        {
            _providers.ForEach(_ => command.AddCommand(_.Provide()));
        }
    }
}
