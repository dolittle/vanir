// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Dolittle.Vanir.Backend.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Extension methods for working with <see cref="Features"/>.
    /// </summary>
    public static class FeaturesExtensions
    {
        /// <summary>
        /// Convert <see cref="Features"/> to JSON.
        /// </summary>
        /// <param name="features"><see cref="IDictionary<string, Feature>"/> to convert.</param>
        /// <returns>JSON result.</returns>
        public static string ToJSON(this IDictionary<string, Feature> features)
        {
            var definitions = features.ToDefinitions();
            var container = definitions.ToDictionary(_ => _.Name, _ => _);
            container.ForEach(kvp => kvp.Value.Name = null);

            return JsonConvert.SerializeObject(container, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        /// <summary>
        /// Convert <see cref="Features"/> to a collection of <see cref="FeatureDefinition"/>
        /// </summary>
        /// <param name="features"><see cref="IDictionary<string, Feature>"/> to convert.</param>
        /// <returns>Collection of <see cref="FeatureDefinition"/>.</returns>
        public static IEnumerable<FeatureDefinition> ToDefinitions(this IDictionary<string, Feature> features)
        {
            return features.Values.Select(_ => new FeatureDefinition
            {
                Name = _.Name,
                Description = _.Description,
                Toggles = _.Toggles.Select(_ => new FeatureToggleDefinition
                {
                    Type = "Boolean",
                    IsOn = _.IsOn
                }).ToArray()
            }).ToArray();
        }
    }
}
