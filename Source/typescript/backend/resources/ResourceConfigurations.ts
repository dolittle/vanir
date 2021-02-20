// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import fs from 'fs';
import { injectable, singleton } from 'tsyringe';
import { TenantId } from '@dolittle/sdk.execution';
import { ResourceConfiguration } from './ResourceConfiguration';
import { Constructor } from '@dolittle/types';
import { IResourceConfigurations } from './IResourceConfigurations';
import { MissingResourceConfigurationsForTenant } from './MissingResourceConfigurationsForTenant';
import { MissingResourceConfigurationOfType } from './MissingResourceConfigurationOfType';
import { ResourcesFileStructure } from './ResourcesFileStructure';
import { EventStoreConfiguration } from './EventStoreConfiguration';
import { MongoDbReadModelsConfiguration } from '../mongodb';

const ResourcesJsonFilePath = '.dolittle/resources.json';

/**
 * Represents an implementation of {@link IResourceConfigurations}
 */
@injectable()
@singleton()
export class ResourceConfigurations implements IResourceConfigurations {

    private readonly _resourceConfigurationsByTenants: Map<string, Map<Constructor, any>> = new Map();

    constructor() {
        this.populate();
    }

    /** @inheritdoc */
    getFor<TResource extends ResourceConfiguration>(resourceConfigurationType: Constructor<TResource>, tenantId: TenantId): TResource {
        this.ThrowIfMissingResourceConfigurationsForTenant(tenantId);

        const resourceConfigurations = this._resourceConfigurationsByTenants.get(tenantId.toString());
        this.ThrowIfMissingResourceConfigurationOfType<TResource>(resourceConfigurations, resourceConfigurationType, tenantId);

        return resourceConfigurations?.get(resourceConfigurationType) as TResource;
    }


    private populate() {
        if (fs.existsSync(ResourcesJsonFilePath)) {
            const resourcesJsonAsString = fs.readFileSync(ResourcesJsonFilePath).toString();
            const resourcesJson = JSON.parse(resourcesJsonAsString) as ResourcesFileStructure;
            for (const tenantIdString in resourcesJson) {
                const tenantId = TenantId.from(tenantIdString);
                const tenantResources = resourcesJson[tenantIdString];

                const resourceConfigurationsMap: Map<Constructor, any> = new Map();
                this._resourceConfigurationsByTenants.set(tenantId.toString(), resourceConfigurationsMap);

                for (const resourceType in tenantResources) {
                    switch (resourceType) {
                        case 'eventStore': {
                            resourceConfigurationsMap.set(EventStoreConfiguration, tenantResources[resourceType]);
                        } break;
                        case 'readModels': {
                            resourceConfigurationsMap.set(MongoDbReadModelsConfiguration, tenantResources[resourceType]);
                        } break;
                    }
                }
            }
        }
    }

    private ThrowIfMissingResourceConfigurationOfType<TResource extends ResourceConfiguration>(resourceConfigurations: Map<Constructor<{}>, any> | undefined, resourceConfigurationType: Constructor<TResource>, tenantId: TenantId) {
        if (!resourceConfigurations || !resourceConfigurations.has(resourceConfigurationType)) {
            throw new MissingResourceConfigurationOfType(tenantId, resourceConfigurationType);
        }
    }

    private ThrowIfMissingResourceConfigurationsForTenant(tenantId: TenantId) {
        if (!this._resourceConfigurationsByTenants.has(tenantId.toString())) {
            throw new MissingResourceConfigurationsForTenant(tenantId);
        }
    }
}


