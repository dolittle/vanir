// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using HotChocolate.Types;

namespace Dolittle.Vanir.Backend.GraphQL
{
    /// <summary>
    /// Represents the definition of mutations based on discovery of <see cref="GraphControllers"/> and
    /// usage of the <see cref="MutationAttribute"/> attribute.
    /// </summary>
    public class MutationType : ObjectType<Mutation>
    {
        readonly IGraphControllers _graphControllers;

        /// <summary>
        /// Initializes a new instance of <see cref="MutationType"/>
        /// </summary>
        /// <param name="graphControllers"><see cref="IGraphControllers"/> to get controllers.</param>
        public MutationType(IGraphControllers graphControllers)
        {
            _graphControllers = graphControllers;
        }

        /// <inheritdoc/>
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            var hasFields = false;

            foreach (var type in _graphControllers.All)
            {
                var addedFields = descriptor.AddMutationFields(type);
                if (addedFields.Any()) hasFields = true;
            }

            if (!hasFields) descriptor.Field("Default").Resolve(() => "Configure your first query");

            base.Configure(descriptor);
        }
    }
}
