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
    /// <summary>
    /// Represents an implementation of <see cref="IExecutionContextManager"/>.
    /// </summary>
    public class ExecutionContextManager : IExecutionContextManager
    {
        static readonly AsyncLocal<ExecutionContext> _currentExecutionContext = new();

        /// <summary>
        /// Get current <see cref="ExecutionContext"/>.
        /// </summary>
        /// <returns>Current <see cref="ExecutionContext"/>.</returns>
        public static ExecutionContext GetCurrent() => _currentExecutionContext.Value;

        /// <summary>
        /// Set a <see cref="ExecutionContext"/> for current call path.
        /// </summary>
        /// <param name="context"><see cref="ExecutionContext"/> to set.</param>
        public static void SetCurrent(ExecutionContext context) => _currentExecutionContext.Value = context;

        /// <inheritdoc/>
        public ExecutionContext Current => _currentExecutionContext.Value;

        /// <inheritdoc/>
        public ExecutionContext Establish(TenantId tenantId, CorrelationId correlationId)
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

        /// <inheritdoc/>
        public void Set(ExecutionContext context) => _currentExecutionContext.Value = context;
    }
}
