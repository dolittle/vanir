// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Dolittle.Vanir.CLI
{
    /// <summary>
    /// Defines an interface that can provide a <see cref="Command"/> to be exposed for the CLI.
    /// </summary>
    public interface ICanProvideCommand
    {
        /// <summary>
        /// Provide a command.
        /// </summary>
        /// <returns><see cref="Command"/> provided.</returns>
        Command Provide();
    }
}
