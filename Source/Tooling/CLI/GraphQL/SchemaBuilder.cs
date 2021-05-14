// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.Reflection;
using Types = Dolittle.Vanir.CLI.Reflection.Types;

namespace Dolittle.Vanir.CLI.GraphQL
{
    /// <summary>
    /// Represents an implementation of <see cref="ISchemaBuilder"/>
    /// </summary>
    public class SchemaBuilder : ISchemaBuilder
    {
        /// <inheritdoc/>
        public Schema BuildFrom(Types types)
        {
            var graphControllers = types.Where(_ => _.Implements(typeof(GraphController)));
            var mutationTypes = graphControllers.GetMutationTypes();
            //var queryTypes = graphControllers.GetQueryTypes();

            throw new System.NotImplementedException();
        }
    }
}
