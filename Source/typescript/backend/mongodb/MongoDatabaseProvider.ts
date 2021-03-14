// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TenantId } from '@dolittle/sdk.execution';
import { MongoClient } from 'mongodb';
import { IResourceConfigurations } from '../resources';
import { MongoDatabase } from './MongoDatabase';
import { MongoDbReadModelsConfiguration } from './MongoDbReadModelsConfiguration';
import { IMongoDatabase } from './IMongoDatabase';
import { injectable } from 'tsyringe';

/**
 * Represents a system that can provide {@link IMongoDatabase} for the context you need.
 */
@injectable()
export class MongoDatabaseProvider {
    constructor(private readonly _resourceConfigurations: IResourceConfigurations) {
    }

    /**
     * Get a {@link IMongoDatabase} instance for a specific {@link TenantId}.
     * @param {TenantId} tenantId Tenant to get for
     * @returns IMongoDatabase
     */
    getFor(tenantId: TenantId): IMongoDatabase {
        const configuration = this._resourceConfigurations.getFor(MongoDbReadModelsConfiguration, tenantId);
        const client = new MongoClient(configuration.host, { useNewUrlParser: true, useUnifiedTopology: true });
        const database = new MongoDatabase(client, configuration);
        return database;
    }
}
