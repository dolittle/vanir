// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
using Autofac;

namespace Dolittle.Vanir.CLI
{
    static class Program
    {
        static int Main(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            IContainer container = null;
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterSource<SelfBindingRegistrationSource>();
            containerBuilder.RegisterAssemblyModules(assembly);
            containerBuilder.Register(_ => container).As<IContainer>().SingleInstance();
            container = containerBuilder.Build();

            var rootCommand = new RootCommand
            {
                Description = "Vanir Command Line Tool"
            };

            container.Resolve<CommandProviders>().AddAllCommandsFromProvidersTo(rootCommand);
            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
