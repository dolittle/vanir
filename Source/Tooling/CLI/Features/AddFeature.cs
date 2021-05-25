// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Features;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.Features
{
    public class AddFeature : AlterFeaturesCommandHandler
    {
        public const string NameArgument = "name";
        public const string DescriptionArgument = "description";

        public AddFeature(ContextOf<ApplicationContext> getApplicationContext,
            ContextOf<MicroserviceContext> getMicroserviceContext,
            ContextOf<FeaturesContext> getFeaturesContext) : base(
                getApplicationContext,
                getMicroserviceContext,
                getFeaturesContext
            )
        {
        }

        protected override Task<int> Invoke(InvocationContext context, IDictionary<string, Feature> features)
        {
            var name = context.ParseResult.ValueForArgument<string>(NameArgument);
            var description = context.ParseResult.ValueForArgument<string>(DescriptionArgument);

            if (features.Values.Any(_ => _.Name == name))
            {
                context.Console.Error.Write($"Feature '{name}' already exists");
                return Task.FromResult(-1);
            }

            features.Add(name,
                    new Feature
                    {
                        Name = name,
                        Description = description,
                        Toggles = new[] {
                            new BooleanFeatureToggle()
                        }
                    });

            return Task.FromResult(0);
        }
    }
}
