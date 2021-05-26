// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Dolittle.SDK.Tenancy;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.Tenants
{
    public class Init : ICommandHandler
    {
        readonly ContextOf<MicroserviceContext> _getMicroserviceContext;

        public Init(ContextOf<MicroserviceContext> getMicroserviceContext)
        {
            _getMicroserviceContext = getMicroserviceContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var microserviceContext = _getMicroserviceContext();
            var tenants = microserviceContext.GetTenants();
            microserviceContext.SaveTenants(new Guid[] { TenantId.Development });
            var view = new ListTenantsView(_getMicroserviceContext());
            context.Render(view);

            return Task.FromResult(0);
        }
    }
}
