// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Vanir.Backend.Execution;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ExecutionContextServiceCollectionExtensions
    {
        public static void AddExecutionContext(this IServiceCollection services)
        {
            services.AddSingleton<IExecutionContextManager>(new ExecutionContextManager());
        }
    }
}
