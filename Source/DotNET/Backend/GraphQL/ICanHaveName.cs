// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Defines something that has a name.
    /// </summary>
    public interface ICanHaveName
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Check whether or not the name is set.
        /// </summary>
        bool HasName { get; }
    }
}
