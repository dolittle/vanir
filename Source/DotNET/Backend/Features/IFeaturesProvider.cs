// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.Features
{
    /// <summary>
    /// Defines a system that can provide <see cref="Features"/>.
    /// </summary>
    public interface IFeaturesProvider
    {
        /// <summary>
        /// Provide <see cref="Features"/>.
        /// </summary>
        /// <returns>All <see cref="Features"/>.</returns>
        Features Provide();
    }
}
