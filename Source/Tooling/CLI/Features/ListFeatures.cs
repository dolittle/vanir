// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Features;

namespace Dolittle.Vanir.CLI.Features
{
    public class ListFeatures : ICommandHandler
    {
        readonly ApplicationContext _applicationContext;
        readonly MicroserviceContext _microserviceContext;
        readonly FeaturesContext _featuresContext;

        public ListFeatures(
            ApplicationContext applicationContext,
            MicroserviceContext microserviceContext,
            FeaturesContext featuresContext)
        {
            _applicationContext = applicationContext;
            _microserviceContext = microserviceContext;
            _featuresContext = featuresContext;
        }

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
                Items = _featuresContext.Features.Values.ToArray()
            };

            table.AddColumn(_ => _.Name, new ContentView("NAME".Underline()));
            table.AddColumn(_ => _.Description, new ContentView("DESCRIPTION".Underline()));
            table.AddColumn(new IsOnColumn());

            table.Render(consoleRenderer, region);

            return Task.FromResult(0);
        }
    }
}
