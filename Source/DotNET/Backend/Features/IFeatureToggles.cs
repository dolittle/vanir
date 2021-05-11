// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Defines an interface for checking if a feature is on or off.
    /// </summary>
    public interface IFeatureToggles
    {
        /// <summary>
        /// Check if a feature is on or off.
        /// </summary>
        /// <param name="feature">Feature identifier.</param>
        /// <returns>True if on, false it off.</returns>
        /// <remarks>If the feature is not configured, it will return false.</remarks>
        bool IsFeatureOn(string feature);
    }
}
