// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.CLI.Reflection;

namespace Dolittle.Vanir.CLI.GraphQL
{
    /// <summary>
    /// Defines a system for building GraphQL <see cref="Schema"/> objects
    /// </summary>
    public interface ISchemaBuilder
    {
        /// <summary>
        /// Build a <see cref="Schema"/> from <see cref="Types"/>
        /// </summary>
        /// <param name="type"><see cref="Types"/> to build from.</param>
        /// <returns>Built <see cref="Schema"/></returns>
        Schema BuildFrom(Types type);
    }
}
