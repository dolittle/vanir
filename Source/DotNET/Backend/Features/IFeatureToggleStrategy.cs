// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Defines a strategy for toggling a <see cref="FeatureDefinition"/>.
    /// </summary>
    public interface IFeatureToggleStrategy
    {
        /// <summary>
        /// Get whether or not the feature is on.
        /// </summary>
        bool IsOn { get; }
    }
}
