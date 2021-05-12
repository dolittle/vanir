// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Autofac;
using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.Reflection;

namespace Dolittle.Vanir.CLI
{
    public class CommandProvidersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(Program).Assembly;
            var commandProviders = assembly.DefinedTypes.Where(_ => _.HasInterface<ICanProvideCommand>());
            commandProviders.ForEach(_ => builder.RegisterType(_).AsSelf());
            builder.RegisterInstance(new CommandProviders(commandProviders.ToArray())).AsSelf().SingleInstance();
        }
    }
}
