// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Dolittle.Vanir.CLI.GraphQL
{
    /// <summary>
    /// Represents a GraphQL schema
    /// </summary>
    public class Schema
    {
        public IEnumerable<OperationDefinition> Mutations { get; init; }
        public IEnumerable<OperationDefinition> Queries { get; init; }
        public IEnumerable<TypeDefinition> Types { get; init; }
    }
}
