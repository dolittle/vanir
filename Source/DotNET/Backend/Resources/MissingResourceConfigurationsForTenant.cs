// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.SDK.Tenancy;

namespace Dolittle.Vanir.Backend.Resources
{
    public class MissingResourceConfigurationsForTenant : Exception
    {
        public MissingResourceConfigurationsForTenant(TenantId tenantId)
            : base($"Missing resource configurations for '{tenantId}'")
        {
        }
    }
}
