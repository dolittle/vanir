// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dolittle.Vanir.Backend
{
    /// <summary>
    /// Represents an implementation of <see cref="IContainer"/>.
    /// </summary>
    public class Container : IContainer
    {
        internal static IServiceProvider ServiceProvider;

        /// <inheritdoc/>
        public T Get<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }

        /// <inheritdoc/>
        public object Get(Type type)
        {
            return ServiceProvider.GetService(type);
        }
    }
}
