// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using Dolittle.SDK.Tenancy;
using Dolittle.Vanir.Backend.Execution;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExecutionContextAppBuilderExtensions
    {
        const string _tenantId = "Tenant-ID";

        public static void UseExecutionContext(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                var tenantId = TenantId.Development;

                if (context.Request.Headers.ContainsKey(_tenantId))
                {
                    tenantId = context.Request.Headers[_tenantId].First();
                }

                var executionContextManager = app.ApplicationServices.GetService(typeof(IExecutionContextManager)) as IExecutionContextManager;
                executionContextManager.Establish(tenantId, Guid.NewGuid());
                await next.Invoke().ConfigureAwait(false);
            });
        }
    }
}
