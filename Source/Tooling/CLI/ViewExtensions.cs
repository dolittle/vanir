// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;

namespace Dolittle.Vanir.CLI
{
    public static class InvocationContextExtensions
    {
        public static void Render(this InvocationContext context, View view)
        {
            var region = new Region(0, 0, Console.WindowWidth, Console.WindowHeight, true);

            var consoleRenderer = new ConsoleRenderer(
                context.Console,
                mode: OutputMode.Auto,
                resetAfterRender: true
            );

            view.Render(consoleRenderer, region);
        }
    }
}
