// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend
{
    /// <summary>
    /// Defines a container to be used internally by Vanir.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Get the instance of a type.
        /// </summary>
        /// <typeparam name="T">Type to get instance of</typeparam>
        /// <returns>The instance.</returns>
        T Get<T>();

        /// <summary>
        /// Get the instance of a type.
        /// </summary>
        /// <param name="type">Type to get instance of.</param>
        /// <returns>The instance.</returns>
        object Get(Type type);
    }
}
