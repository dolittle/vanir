// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Defines a system that can parse features from JSON.
    /// </summary>
    public interface IFeaturesParser
    {
        /// <summary>
        /// Parse a JSON representation.
        /// </summary>
        /// <param name="json">JSON as string to parse.</param>
        /// <returns><see cref="Features"/> instance.</returns>
        Features Parse(string json);
    }
}
