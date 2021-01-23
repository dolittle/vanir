// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Execution;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExecutionContextExtensions
    {
        public static void UseExecutionContext(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                ExecutionContextManager.Establish();
                await next.Invoke().ConfigureAwait(false);
            });
        }
    }
}
