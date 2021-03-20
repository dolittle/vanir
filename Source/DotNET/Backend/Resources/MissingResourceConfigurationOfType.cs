// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Dolittle.SDK.Tenancy;

namespace Dolittle.Vanir.Backend.Resources
{
    public class MissingResourceConfigurationOfType : Exception
    {
        public MissingResourceConfigurationOfType(TenantId tenantId, Type type)
            : base($"Missing resource configuration of type '{type.Name}' for tenant '${tenantId}'")
        {
        }
    }

}
