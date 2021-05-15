// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Represents an implementation of <see cref="IFeatureToggleStrategy"/> for a simple true / false scenario.
    /// </summary>
    public class BooleanFeatureToggleStrategy : IFeatureToggleStrategy
    {
        /// <inheritdoc/>
        public bool IsOn { get; init; }
    }
}
