// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Dolittle.Vanir.Backend.Features;
using HotChocolate.Configuration;
using HotChocolate.Types.Descriptors.Definitions;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Represents a <see cref="TypeInterceptor"/> that alters the schema according to whether or not features are enabled.
    /// </summary>
    public class FeatureTogglesInterceptor : TypeInterceptor
    {
        readonly IFeatureToggles _featureToggles;

        /// <summary>
        /// Initializes a new instance of <see cref="FeatureTogglesInterceptor"/>.
        /// </summary>
        /// <param name="featureToggles"><see cref="IFeatureToggles"/> to use.</param>
        public FeatureTogglesInterceptor(IFeatureToggles featureToggles)
        {
            _featureToggles = featureToggles;
        }

        /// <inheritdoc/>
        public override bool CanHandle(ITypeSystemObjectContext context)
        {
            return context.Type is SchemaRoute sr && sr.HasItems;
        }

        /// <inheritdoc/>
        public override void OnBeforeCompleteType(ITypeCompletionContext completionContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
            if (definition is ObjectTypeDefinition otd)
            {
                var fieldsToRemove = new List<ObjectFieldDefinition>();

                foreach (var field in otd.Fields.Where(_ => _.Member != null))
                {
                    var featureAttributes = new List<FeatureAttribute>();
                    featureAttributes.AddRange(field.Member.GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[]);
                    featureAttributes.AddRange(field.Member.DeclaringType.GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[]);

                    foreach (var featureAttribute in featureAttributes)
                    {
                        if (!_featureToggles.IsOn(featureAttribute.Name))
                        {
                            fieldsToRemove.Add(field);
                            break;
                        }
                    }
                }

                fieldsToRemove.ForEach(_ => otd.Fields.Remove(_));
            }
        }
    }
}
