// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Config;

namespace Dolittle.Vanir.CLI
{
    /// <summary>
    /// Delegate that can be used as dependency for getting a specific context.
    /// </summary>
    /// <typeparam name="T">Type of context to get.</typeparam>
    /// <returns>Instance of the context.</returns>
    /// <remarks>
    /// The contexts are typically things like <see cref="Microservice"/> or <see cref="Application"/>.
    /// If the CLI is not in the context of what is being asked for, an error message will be presented.
    /// </remarks>
    public delegate T ContextOf<T>();
}
