// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Represents a set of <see cref="Features"/>.
    /// </summary>
    public class Features : ReadOnlyDictionary<string, IFeatureToggleStrategy>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Features"/> without any features.
        /// </summary>
        public Features() : base(new Dictionary<string, IFeatureToggleStrategy>())
        {
        }

        /// <summary>
        /// Initializes a new instance <see cref="Features"/> and populates with the given features.
        /// </summary>
        /// <param name="featureToggles">Feature toggles to initialize with.</param>
        public Features(IDictionary<string, IFeatureToggleStrategy> featureToggles) : base(featureToggles)
        {
        }
    }
}
