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
        readonly ContextOf<ApplicationContext> _getApplicationContext;

        public Init(ContextOf<ApplicationContext> getApplicationContext)
        {
            _getApplicationContext = getApplicationContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var microserviceContext = _getApplicationContext();
            var tenants = microserviceContext.GetTenants();
            microserviceContext.SaveTenants(new Guid[] { TenantId.Development });
            var view = new ListTenantsView(_getApplicationContext());
            context.Render(view);

            return Task.FromResult(0);
        }
    }
}
