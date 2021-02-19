// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TenantId } from '@dolittle/sdk.execution';
import { ResourceConfiguration } from './ResourceConfiguration';
import { Constructor } from '@dolittle/types';
import { IResourceConfigurations } from './IResourceConfigurations';
import { injectable, singleton } from 'tsyringe';
import { MissingResourceConfigurationsForTenant } from './MissingResourceConfigurationsForTenant';
import { MissingResourceConfigurationOfType } from './MissingResourceConfigurationOfType';

/**
 * Represents an implementation of {@link IResourceConfigurations}
 */
@injectable()
@singleton()
export class ResourceConfigurations implements IResourceConfigurations {
    private readonly _resourceConfigurationsByTenants: Map<TenantId, Map<Constructor, any>> = new Map();

    constructor() {

    }

    /** @inheritdoc */
    getFor<TResource extends ResourceConfiguration>(resourceConfigurationType: Constructor<TResource>, tenantId: TenantId): TResource {
        this.ThrowIfMissingResourceConfigurationsForTenant<TResource>(tenantId);

        const resourceConfigurations = this._resourceConfigurationsByTenants.get(tenantId);
        this.ThrowIfMissingResourceConfigurationOfType<TResource>(resourceConfigurations, resourceConfigurationType, tenantId);

        return resourceConfigurations?.get(resourceConfigurationType) as TResource;
    }


    private populate() {
        // Read the .dolittle/resources.json and convert into our map
    }

    private ThrowIfMissingResourceConfigurationOfType<TResource extends ResourceConfiguration>(resourceConfigurations: Map<Constructor<{}>, any> | undefined, resourceConfigurationType: Constructor<TResource>, tenantId: TenantId) {
        if (!resourceConfigurations || !resourceConfigurations.has(resourceConfigurationType)) {
            throw new MissingResourceConfigurationOfType(tenantId, resourceConfigurationType);
        }
    }

    private ThrowIfMissingResourceConfigurationsForTenant<TResource extends ResourceConfiguration>(tenantId: TenantId) {
        if (!this._resourceConfigurationsByTenants.has(tenantId)) {
            throw new MissingResourceConfigurationsForTenant(tenantId);
        }
    }
}


