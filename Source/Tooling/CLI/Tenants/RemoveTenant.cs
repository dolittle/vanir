// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.Vanir.CLI.Contexts;

namespace Dolittle.Vanir.CLI.Tenants
{
    public class RemoveTenant : ICommandHandler
    {
        public const string Id = "id";
        readonly ContextOf<ApplicationContext> _getApplicationContext;

        public RemoveTenant(ContextOf<ApplicationContext> getApplicationContext)
        {
            _getApplicationContext = getApplicationContext;
        }

        public Task<int> InvokeAsync(InvocationContext context)
        {
            var tenantId = context.ParseResult.ValueForArgument<Guid>(Id);
            var applicationContext = _getApplicationContext();
            var tenants = applicationContext.GetTenants();

            if (!tenants.Any(_ => _ == tenantId))
            {
                context.Console.Error.Write($"Tenant '{tenantId}' does not exist and can't be removed.");
                Environment.Exit(0);
                return Task.FromResult(0);
            }

            tenants = tenants.Where(_ => _ != tenantId).ToArray();
            applicationContext.SaveTenants(tenants);

            var view = new ListTenantsView(_getApplicationContext());
            context.Render(view);

            return Task.FromResult(0);
        }
    }
}
