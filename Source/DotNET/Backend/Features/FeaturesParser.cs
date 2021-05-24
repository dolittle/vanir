// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Represents an implementation of <see cref="IFeaturesParser"/>
    /// </summary>
    public class FeaturesParser : IFeaturesParser
    {
        /// <inheritdoc/>
        public Features Parse(string json)
        {
            var featureDefinitions = JsonConvert.DeserializeObject<Dictionary<string, FeatureDefinition>>(json);
            var features = featureDefinitions.ToDictionary(
                _ => _.Key,
                _ => new Feature
                {
                    Name = _.Key,
                    Description = _.Value.Description,
                    Toggles = _.Value.Toggles.Select(t => new BooleanFeatureToggle { IsOn = t.IsOn })
                });
            return new Features(features);
        }
    }
}
