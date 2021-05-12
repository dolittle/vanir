// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using Autofac;

namespace Dolittle.Vanir.CLI
{

    static class Program
    {
        internal static IContainer Container { get; private set; }

        static int Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterAssemblyModules(assembly);
            Container = containerBuilder.Build();

            var rootCommand = new RootCommand
            {
                Description = "Vanir Command Line Tool"
            };

            Container.Resolve<CommandProviders>().AddAllCommandsFromProvidersTo(rootCommand);

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
