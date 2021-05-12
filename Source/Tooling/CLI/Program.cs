// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using System.Linq;
using Autofac;
using Dolittle.Vanir.Backend.Collections;
using Dolittle.Vanir.Backend.Reflection;

namespace Dolittle.Vanir.CLI
{
    static class Program
    {
        internal static IContainer Container { get; private set; }

        static int Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            var commandProviders = assembly.DefinedTypes.Where(_ => _.HasInterface<ICanProvideCommand>());

            var containerBuilder = new ContainerBuilder();
            commandProviders.ForEach(_ => containerBuilder.RegisterType(_).AsSelf());
            containerBuilder.RegisterAssemblyModules(assembly);
            Container = containerBuilder.Build();

            var rootCommand = new RootCommand
            {
                Description = "Vanir Command Line Tool"
            };

            foreach( var commandProviderType in commandProviders )
            {
                var commandProvider = Container.Resolve(commandProviderType) as ICanProvideCommand;
                rootCommand.Add(commandProvider.Provide());
            }

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
