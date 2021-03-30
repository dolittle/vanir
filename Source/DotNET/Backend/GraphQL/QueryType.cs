// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using HotChocolate.Types;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Represents the definition of queries based on discovery of <see cref="GraphControllers"/> and
    /// usage of the <see cref="QueryAttribute"/> attribute.
    /// </summary>
    public class QueryType : ObjectType<Query>
    {
        readonly IGraphControllers _graphControllers;

        /// <summary>
        /// Initializes a new instance of <see cref="QueryType"/>
        /// </summary>
        /// <param name="graphControllers"><see cref="IGraphControllers"/> to get controllers.</param>
        public QueryType(IGraphControllers graphControllers)
        {
            _graphControllers = graphControllers;
        }

        /// <inheritdoc/>
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            var hasFields = false;

            foreach (var type in _graphControllers.All)
            {
                var addedFields = descriptor.AddQueryFields(type);
                if (addedFields.Any()) hasFields = true;
            }

            if (!hasFields) descriptor.Field("Default").Resolve(() => "Configure your first query");

            base.Configure(descriptor);
        }
    }
}
