// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine.Invocation;
using System.IO;

namespace System.CommandLine
{
    public static class ProxyCommands
    {
        public static void AddProxyCommands(this RootCommand rootCommand)
        {
            var proxyCommand = new Command("proxy", "Generate proxy objects")
            {
                new Argument<FileInfo>("assembly", description:"The source assembly to generate from."),
                new Argument<DirectoryInfo>("outputPath", description: "The output path for the generated proxies.")
            };
            proxyCommand.Handler = CommandHandler.Create<FileInfo, DirectoryInfo>((assembly, outputPath) =>
            {
                Console.WriteLine(assembly.FullName);
                Console.WriteLine(outputPath.FullName);
            });
            rootCommand.AddCommand(proxyCommand);
        }
    }
}
