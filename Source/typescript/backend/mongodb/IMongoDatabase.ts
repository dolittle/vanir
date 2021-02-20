// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Collection, DbCollectionOptions } from 'mongodb';
import { Constructor } from '@dolittle/types';

export abstract class IMongoDatabase {

    abstract collection<TSchema = any>(name: string): Promise<Collection<TSchema>>;
    abstract collection<TSchema = any>(type: Constructor<TSchema>): Promise<Collection<TSchema>>;
    abstract collection<TSchema = any>(type: Constructor<TSchema>, options: DbCollectionOptions): Promise<Collection<TSchema>>;
    abstract collection<TSchema = any>(type: Constructor<TSchema>, name: string): Promise<Collection<TSchema>>;
    abstract collection<TSchema = any>(type: Constructor<TSchema>, name: string, options: DbCollectionOptions): Promise<Collection<TSchema>>;
}


