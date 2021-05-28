// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Dolittle.Vanir.Backend.Features;
using Dolittle.Vanir.Backend.GraphQL;

namespace Backend.Ingress
{
    [GraphRoot("ingress")]
    public class Mutations : GraphController
    {
        [Mutation]
        [Feature("materials")]
        public Task<bool> AddMaterial(string materialId)
        {
            return Task.FromResult(true);
        }
    }
}
