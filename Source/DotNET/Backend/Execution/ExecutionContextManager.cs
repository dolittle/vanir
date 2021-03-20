// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using System.Threading;
using Dolittle.SDK.Execution;
using Dolittle.SDK.Microservices;
using Dolittle.SDK.Security;
using Dolittle.SDK.Tenancy;
using Dolittle.Vanir.Backend.Config;
using Dolittle.Vanir.Backend.Versioning;
using ExecutionContext = Dolittle.SDK.Execution.ExecutionContext;

namespace Dolittle.Vanir.Backend.Execution
{
    public static class ExecutionContextManager
    {
        static readonly AsyncLocal<ExecutionContext> _currentExecutionContext = new();
        public static ExecutionContext Current => _currentExecutionContext.Value;

        public static ExecutionContext Establish(TenantId tenantId, CorrelationId correlationId)
        {
            _currentExecutionContext.Value = new ExecutionContext(
                MicroserviceManager.Current.Id,
                tenantId,
                VersionConverter.FromString(MicroserviceManager.Current.Version),
                Environment.Development,
                correlationId,
                Claims.Empty,
                CultureInfo.InvariantCulture);

            return _currentExecutionContext.Value;
        }

        public static void Set(ExecutionContext context) => _currentExecutionContext.Value = context;
    }
}
