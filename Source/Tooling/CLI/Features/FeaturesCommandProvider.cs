// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Threading.Tasks;
using Dolittle.Vanir.CLI;

namespace CLI.Features
{

    internal static class Colorizer
    {
        public static TextSpan Underline(this string value) =>
            new ContainerSpan(StyleSpan.UnderlinedOn(),
                              new ContentSpan(value),
                              StyleSpan.UnderlinedOff());

        public static TextSpan Rgb(this string value, byte r, byte g, byte b) =>
            new ContainerSpan(ForegroundColorSpan.Rgb(r, g, b),
                              new ContentSpan(value),
                              ForegroundColorSpan.Reset());

        public static TextSpan LightGreen(this string value) =>
            new ContainerSpan(ForegroundColorSpan.LightGreen(),
                              new ContentSpan(value),
                              ForegroundColorSpan.Reset());

        public static TextSpan White(this string value) =>
            new ContainerSpan(ForegroundColorSpan.White(),
                              new ContentSpan(value),
                              ForegroundColorSpan.Reset());
    }

    public class Feature
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ListFeatures : ICommandHandler
    {
        public Task<int> InvokeAsync(InvocationContext context)
        {
            var region = new Region(0, 0, Console.WindowWidth, Console.WindowHeight, true);

            var consoleRenderer = new ConsoleRenderer(
                context.Console,
                mode: context.BindingContext.OutputMode(),
                resetAfterRender: true
            );

            var table = new TableView<Feature>
            {
                Items = new Feature[]
                {
                    new() { Name = "Something", Description = "Descriptions are awesome" },
                    new() { Name = "More stuff", Description = "Kakkelovn" },
                    new() { Name = "Kattefunksjonen", Description = "Møterapport" }
                }
            };

            table.AddColumn(_ => _.Name, new ContentView("NAME".Underline()));
            table.AddColumn(_ => _.Description, new ContentView("DESCRIPTION".Underline()));

            table.Render(consoleRenderer, region);

            return Task.FromResult(0);
        }
    }

    public class FeaturesCommandProvider : ICanProvideCommand
    {
        public Command Provide()
        {
            var command = new Command("features", "Work with features in the system")
            {
                new Option<string>("environment", description: "Environment to apply to (defaults to local)")
            };

            var listCommand = new Command("list", "List all features");
            listCommand.Handler = new ListFeatures();
            command.AddCommand(listCommand);

            var addCommand = new Command("add", "Add a feature")
            {
                new Argument<string>("name", description: "Name of the feature"),
                new Option<string>("description", description: "Description of the feature")
            };
            command.AddCommand(addCommand);

            var removeCommand = new Command("remove", "Remove a feature")
            {
                new Argument<string>("name", description: "Name of the feature")
            };
            command.AddCommand(removeCommand);

            var toggleCommand = new Command("toggle", "Toggle on or off a feature")
            {
                new Argument<string>("feature", description: "Name of the feature to toggle")
            };
            command.AddCommand(toggleCommand);

            return command;
        }
    }
}
