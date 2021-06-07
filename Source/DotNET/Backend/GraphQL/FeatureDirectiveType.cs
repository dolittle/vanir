// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Features;
using HotChocolate.Types;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Represents the type for the <see cref="FeatureDirective">@feature directive</see>.
    /// </summary>
    public class FeatureDirectiveType : DirectiveType<FeatureDirective>
    {
        /// <summary>
        /// Names used.
        /// </summary>
        public static class Names
        {
            public const string Directive = "feature";
            public const string Name = "name";
        }

        readonly IFeatureToggles _featureToggles;

        /// <summary>
        /// Initializes a new instance of <see cref="FeatureDirectiveType"/>
        /// </summary>
        /// <param name="featureToggles"></param>
        public FeatureDirectiveType(IFeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        /// <inheritdoc/>
        protected override void Configure(IDirectiveTypeDescriptor<FeatureDirective> descriptor)
        {
            descriptor
                .Argument(_ => _.Name)
                .Description("Name of feature")
                .Type<StringType>();

            descriptor
                .Name(Names.Directive)
                .Location(DirectiveLocation.FieldDefinition)
                .Use(next => context =>
                {
                    var feature = context.Directive.ToObject<FeatureDirective>();
                    if (!_featureToggles.IsOn(feature.Name))
                    {
                        context.ReportError($"Feature '{feature.Name}' is disabled");
                        return new ValueTask(Task.FromResult(false));
                    }

                    return next.Invoke(context);
                });
        }
    }
}
