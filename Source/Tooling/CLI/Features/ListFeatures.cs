// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Config;
using Dolittle.Vanir.Backend.Features;

namespace Dolittle.Vanir.CLI.Features
{
    public class ListFeatures : ICommandHandler
    {
        readonly ContextOf<Application> _getApplicationContext;
        readonly ContextOf<Microservice> _getMicroserviceContext;
        readonly ContextOf<Backend.Features.Features> _getFeaturesContext;

        public ListFeatures(
            ContextOf<Application> getApplicationContext,
            ContextOf<Microservice> getMicroserviceContext,
            ContextOf<Backend.Features.Features> getFeaturesContext)
        {
            _getApplicationContext = getApplicationContext;
            _getMicroserviceContext = getMicroserviceContext;
            _getFeaturesContext = getFeaturesContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var application = _getApplicationContext();
            var microservice = _getMicroserviceContext();
            var features = _getFeaturesContext();

            var region = new Region(0, 0, Console.WindowWidth, Console.WindowHeight, true);

            var consoleRenderer = new ConsoleRenderer(
                context.Console,
                mode: context.BindingContext.OutputMode(),
                resetAfterRender: true
            );

            var table = new TableView<Feature>
            {
                Items = features.Values.ToArray()
            };

            table.AddColumn(_ => _.Name, new ContentView("NAME".Underline()));
            table.AddColumn(_ => _.Description, new ContentView("DESCRIPTION".Underline()));
            table.AddColumn(new IsOnColumn());

            table.Render(consoleRenderer, region);

            return Task.FromResult(0);
        }
    }
}
