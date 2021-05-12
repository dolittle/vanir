// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace Dolittle.Vanir.CLI.ProxyGenerator
{
    public class ProxyCommandProvider : ICanProvideCommand
    {
        private readonly IGenerator _generator;

        public ProxyCommandProvider(IGenerator generator)
        {
            _generator = generator;
        }

        public Command Provide()
        {
            var proxyCommand = new Command("proxy", "Generate proxy objects")
            {
                new Argument<FileInfo>("assembly", description:"The source assembly to generate from."),
                new Argument<DirectoryInfo>("outputPath", description: "The output path for the generated proxies.")
            };
            proxyCommand.Handler = CommandHandler.Create<FileInfo, DirectoryInfo>(_generator.Generate);
            return proxyCommand;
        }
    }
}
