// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.SDK.Tenancy;

namespace Dolittle.Vanir.Backend.Resources
{
    /// <summary>
    /// Exception that is thrown when a resource configuration for a specific type is missing.
    /// </summary>
    public class MissingResourceConfigurationOfType : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MissingResourceConfigurationOfType"/>.
        /// </summary>
        /// <param name="tenantId"><see cref="TenantId"/> the configuration is missing for.</param>
        /// <param name="type">Type of resource configuration.</param>
        public MissingResourceConfigurationOfType(TenantId tenantId, Type type)
            : base($"Missing resource configuration of type '{type.Name}' for tenant '${tenantId}'")
        {
        }
    }
}
