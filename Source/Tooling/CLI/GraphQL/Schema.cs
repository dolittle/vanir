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
        IEnumerable<MutationDefinition> Mutations { get; init; }
        IEnumerable<QueryDefinition> Queries { get; init; }
        IEnumerable<ReadModelDefinition> Types { get; init; }
    }
}
