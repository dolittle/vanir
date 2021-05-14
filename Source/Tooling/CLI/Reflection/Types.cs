// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Dolittle.Vanir.CLI.Reflection
{
    /// <summary>
    /// Represents a collection of <see cref="TypeInfo">types</see>.
    /// </summary>
    public class Types : ReadOnlyCollection<TypeInfo>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Types"/>
        /// </summary>
        /// <param name="types">Collection of types</param>
        public Types(IEnumerable<TypeInfo> types) : base(types.ToList())
        {
        }
    }
}
