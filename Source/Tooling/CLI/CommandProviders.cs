// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.CommandLine;
using Autofac;
using Dolittle.Vanir.Backend.Collections;

namespace Dolittle.Vanir.CLI
{
    public class CommandProviders
    {
        readonly IEnumerable<Type> _providerTypes;

        public CommandProviders(IEnumerable<Type> providerTypes)
        {
            _providerTypes = providerTypes;
        }

        public void AddAllCommandsFromProvidersTo(RootCommand command)
        {
            _providerTypes.ForEach(_ =>
            {
                var provider = Program.Container.Resolve(_) as ICanProvideCommand;
                command.AddCommand(provider.Provide());
            });
        }
    }
}
