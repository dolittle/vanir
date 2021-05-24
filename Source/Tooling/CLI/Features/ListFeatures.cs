// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Dolittle.Vanir.CLI.Features
{
    public class ListFeatures : ICommandHandler
    {
        readonly ContextOf<ApplicationContext> _getApplicationContext;
        readonly ContextOf<MicroserviceContext> _getMicroserviceContext;
        readonly ContextOf<FeaturesContext> _getFeaturesContext;

        public ListFeatures(
            ContextOf<ApplicationContext> getApplicationContext,
            ContextOf<MicroserviceContext> getMicroserviceContext,
            ContextOf<FeaturesContext> getFeaturesContext)
        {
            _getApplicationContext = getApplicationContext;
            _getMicroserviceContext = getMicroserviceContext;
            _getFeaturesContext = getFeaturesContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var view = new ListFeaturesView(
                _getApplicationContext(),
                _getMicroserviceContext(),
                _getFeaturesContext().Features.Values);
            context.Render(view);

            return Task.FromResult(0);
        }
    }
}
