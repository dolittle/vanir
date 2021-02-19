// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TenantId } from '@dolittle/sdk.execution';
import { ResourceConfiguration } from './ResourceConfiguration';
import { Constructor } from '@dolittle/types';

/**
 * Defines the system for resource configurations.
 */
export abstract class IResourceConfigurations {

    /**
     * Get resource configuration for a specific type and tenant
     * @param {Constructor<ResourceConfiguration> resourceConfiguration The resource configuration type to get for.
     * @param {TenantId} tenantId TenantId to get for.
     */
    abstract getFor<TResource extends ResourceConfiguration>(resourceConfiguration: Constructor<TResource>, tenantId: TenantId): TResource;
}

