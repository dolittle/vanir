// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.SDK.Tenancy;

namespace Dolittle.Vanir.Backend.Resources
{
    public interface IResourceConfigurations
    {
        TResource GetFor<TResource>(TenantId tenantId) where TResource: ResourceConfiguration;
    }
}
