// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Dolittle.SDK.Tenancy;
using Dolittle.Vanir.Backend.Execution;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExecutionContextExtensions
    {
        const string TENANT_ID = "Tenant-ID";

        public static void UseExecutionContext(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var tenantId = TenantId.Development;

                if (context.Request.Headers.ContainsKey(TENANT_ID))
                {
                    tenantId = context.Request.Headers[TENANT_ID].First();
                }

                ExecutionContextManager.Establish(tenantId, Guid.NewGuid());
                await next.Invoke().ConfigureAwait(false);
            });
        }
    }
}
