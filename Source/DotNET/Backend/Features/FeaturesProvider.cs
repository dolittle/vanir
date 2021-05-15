// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Represents an implementation of <see cref="IFeaturesProvider"/>
    /// </summary>
    public class FeaturesProvider : IFeaturesProvider
    {
        const string _featuresPath = "./data/features.json";

        /// <inheritdoc/>
        public Features Provide()
        {
            if (!File.Exists(_featuresPath)) return new Features();

            var featuresAsJson = File.ReadAllText(_featuresPath);
            var featureBooleans = JsonConvert.DeserializeObject<Dictionary<string, bool>>(featuresAsJson);
            var features = featureBooleans.ToDictionary(
                                _ => _.Key,
                                _ => new BooleanFeatureToggleStrategy { IsOn = _.Value } as IFeatureToggleStrategy);
            return new Features(features);
        }
    }
}
