// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { Collection, DbCollectionOptions, MongoClient } from 'mongodb';
import { injectable } from 'tsyringe';
import { IMongoDatabase } from './IMongoDatabase';
import { MongoDbReadModelsConfiguration } from './MongoDbReadModelsConfiguration';

@injectable()
export class MongoDatabase implements IMongoDatabase {
    constructor(private readonly _mongoClient: MongoClient, private readonly _configuration: MongoDbReadModelsConfiguration) {
    }

    collection<TSchema = any>(name: string): Promise<Collection<TSchema>>
    collection<TSchema = any>(type: Constructor<TSchema>): Promise<Collection<TSchema>>;
    collection<TSchema = any>(type: Constructor<TSchema>, options: DbCollectionOptions): Promise<Collection<TSchema>>;
    collection<TSchema = any>(type: Constructor<TSchema>, name: string): Promise<Collection<TSchema>>;
    collection<TSchema = any>(type: Constructor<TSchema>, name: string, options: DbCollectionOptions): Promise<Collection<TSchema>>;
    async collection<TSchema = any>(typeOrName: Constructor<TSchema> | string, nameOrOption?: string | DbCollectionOptions, options?: DbCollectionOptions): Promise<Collection<TSchema>> {
        let name = (typeOrName instanceof String) ? (typeOrName as string) : (typeOrName as Constructor).name;
        if (nameOrOption) {
            if (nameOrOption instanceof String) {
                name = nameOrOption as string;
            } else {
                options = nameOrOption as DbCollectionOptions;
            }
        }

        await this._mongoClient.connect();
        const db = this._mongoClient.db(this._configuration.database);
        if (options) {
            return db.collection(name, options);
        }
        return db.collection(name);
    }
}
