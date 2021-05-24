// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Features;

namespace Dolittle.Vanir.CLI.Features
{
    public abstract class AlterFeaturesCommandHandler : ICommandHandler
    {
        readonly ContextOf<ApplicationContext> _getApplicationContext;
        readonly ContextOf<MicroserviceContext> _getMicroserviceContext;
        readonly ContextOf<FeaturesContext> _getFeaturesContext;

        protected AlterFeaturesCommandHandler(
            ContextOf<ApplicationContext> getApplicationContext,
            ContextOf<MicroserviceContext> getMicroserviceContext,
            ContextOf<FeaturesContext> getFeaturesContext)
        {
            _getApplicationContext = getApplicationContext;
            _getMicroserviceContext = getMicroserviceContext;
            _getFeaturesContext = getFeaturesContext;
        }

        protected abstract Task<int> Invoke(InvocationContext context, IDictionary<string, Feature> features);

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var featuresContext = _getFeaturesContext();
            var features = new Dictionary<string, Feature>(featuresContext.Features);

            var result = Invoke(context, features);

            var json = features.ToJSON();

            File.WriteAllText(featuresContext.File, json);

            var view = new ListFeaturesView(
                _getApplicationContext(),
                _getMicroserviceContext(),
                features.Values);
            context.Render(view);

            return result;
        }
    }
}
