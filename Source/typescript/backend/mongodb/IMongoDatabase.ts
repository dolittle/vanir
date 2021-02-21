// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Collection, DbCollectionOptions } from 'mongodb';
import { Constructor } from '@dolittle/types';

export abstract class IMongoDatabase {

    /**
     * Get a collection by name
     * @param {string} name Name of collection to get.
     * @returns {Promise<Collection<TSchema>>}
     */
    abstract collection<TSchema = any>(name: string): Promise<Collection<TSchema>>;

    /**
     * Get a collection by the type of schema / document.
     * @param {Constructor} type Type to get for.
     * @returns {Promise<Collection<TSchema>>}
     */
    abstract collection<TSchema = any>(type: Constructor<TSchema>): Promise<Collection<TSchema>>;

    /**
     * Get a collection by the type of schema / document.
     * @param {Constructor} type Type to get for.
     * @param {DbCollectionOptions} options Options to use when getting collection.
     * @returns {Promise<Collection<TSchema>>}
     */
    abstract collection<TSchema = any>(type: Constructor<TSchema>, options: DbCollectionOptions): Promise<Collection<TSchema>>;

    /**
     * Get a collection by the type of schema / document.
     * @param {Constructor} type Type to get for.
     * @param {string} name Name of collection to get.
     * @returns {Promise<Collection<TSchema>>}
     */
    abstract collection<TSchema = any>(type: Constructor<TSchema>, name: string): Promise<Collection<TSchema>>;

    /**
     * Get a collection by the type of schema / document.
     * @param {Constructor} type Type to get for.
     * @param {string} name Name of collection to get.
     * @param {DbCollectionOptions} options Options to use when getting collection.
     * @returns {Promise<Collection<TSchema>>}
     */
    abstract collection<TSchema = any>(type: Constructor<TSchema>, name: string, options: DbCollectionOptions): Promise<Collection<TSchema>>;
}


