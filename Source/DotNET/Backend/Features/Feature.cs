// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Represents a feature in the system.
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// Gets the name of the feature.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Gets the description of the feature.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets all the toggles for the feature.
        /// </summary>
        public IEnumerable<IFeatureToggleStrategy> Toggles { get; init; }

        /// <summary>
        /// Gets whether or not the feature is on.
        /// </summary>
        public bool IsOn => Toggles.Any(_ => _.IsOn);
    }
}
