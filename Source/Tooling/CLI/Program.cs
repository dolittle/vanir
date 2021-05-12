// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;
namespace Dolittle.Vanir.CLI
{
    static class Program
    {
        static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                Description = "Vanir Command Line Tool"
            };

            rootCommand.AddProxyCommands();

            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
