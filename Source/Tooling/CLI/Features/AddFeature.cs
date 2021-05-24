// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Features;

namespace Dolittle.Vanir.CLI.Features
{
    public class AddFeature : ICommandHandler
    {
        public const string NameArgument = "name";
        public const string DescriptionArgument = "description";
        readonly ContextOf<ApplicationContext> _getApplicationContext;
        readonly ContextOf<MicroserviceContext> _getMicroserviceContext;
        readonly ContextOf<FeaturesContext> _getFeaturesContext;


        public AddFeature(ContextOf<ApplicationContext> getApplicationContext,
            ContextOf<MicroserviceContext> getMicroserviceContext,
            ContextOf<FeaturesContext> getFeaturesContext)
        {
            _getApplicationContext = getApplicationContext;
            _getMicroserviceContext = getMicroserviceContext;
            _getFeaturesContext = getFeaturesContext;
        }


        public Task<int> InvokeAsync(InvocationContext context)
        {
            var name = context.ParseResult.ValueForArgument<string>(NameArgument);
            var description = context.ParseResult.ValueForArgument<string>(DescriptionArgument);

            var featuresContext = _getFeaturesContext();
            if (featuresContext.Features.Values.Any(_ => _.Name == name))
            {
                context.Console.Error.Write($"Feature '{name}' already exists");
                return Task.FromResult(-1);
            }

            var features = new Dictionary<string, Feature>(featuresContext.Features)
            {
                {
                    name,
                    new Feature
                    {
                        Name = name,
                        Description = description,
                        Toggles = new[] {
                            new BooleanFeatureToggle()
                        }
                    }
                }
            };

            var json = features.ToJSON();

            File.WriteAllText(featuresContext.File, json);

            var view = new ListFeaturesView(
                _getApplicationContext(),
                _getMicroserviceContext(),
                features.Values);
            context.Render(view);

            return Task.FromResult(0);
        }
    }
}
