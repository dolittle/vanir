// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Invocation;
using System.CommandLine.Rendering;
using System.Threading.Tasks;

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
                mode: OutputMode.Auto,
                resetAfterRender: true
            );

            var view = new ListFeaturesView(_applicationContext, _microserviceContext, _featuresContext.Features.Values);
            view.Render(consoleRenderer, region);

            return Task.FromResult(0);
        }
    }
}
