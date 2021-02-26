// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { Cursor, MongoCallback, MongoError } from 'mongodb';
import { CustomTypes } from './index';
import { getCollectionType } from './MongoDbContext';

declare module 'mongodb' {
    interface Cursor<T> {
        toTypedArray<T>(type: Constructor<T>): Promise<T[]>;
        _next(callback?: MongoCallback<T>): any;
        readBufferedDocuments(count: number): any[];
    }
}

const mapDocument = function <T = any>(type: Constructor<T>, doc: any): T {
    const instance = new type();

    for (const property in doc) {
        (instance as any)[property] = CustomTypes.deserializeProperty(type, property, doc[property]);
    }

    return instance;
};

const _originalNext = Cursor.prototype._next;

Cursor.prototype._next = function (callback?: MongoCallback<any>): any {
    // eslint-disable-next-line @typescript-eslint/no-this-alias
    let cursor = this;
    if (callback) {
        const originalCallback = callback;

        callback = function (error: MongoError, result: any) {
            cursor = cursor;

            const collection = (cursor as any).namespace.collection;
            const collectionType = getCollectionType(collection);
            if (collectionType && result) {
                result = mapDocument(collectionType, result);
            }
            originalCallback(error, result);
        };
    }

    const result = _originalNext.apply(this, [callback]);
    return result;
};


const originalReadBufferedDocuments = Cursor.prototype.readBufferedDocuments;

Cursor.prototype.readBufferedDocuments = function (count: number): any[] {
    const buffered = originalReadBufferedDocuments.apply(this, [count]);

    const collection = (this as any).namespace?.collection || '';
    const collectionType = getCollectionType(collection);
    if (collectionType) {
        const mappedResult = buffered.map(_ => mapDocument(collectionType, _));
        return mappedResult;
    }

    return buffered;
};

/**
 * Convert cursor to a typed array with a given element type.
 * @param {Cursor} this Cursor.
 * @param {Constructor<T>} type Type of array element.
 */
async function toTypedArray<T>(this: Cursor, type: Constructor<T>): Promise<T[]> {
    const result = await this.toArray();
    const mappedResult = result.map(_ => mapDocument(type, _));
    return mappedResult;
}

Cursor.prototype.toTypedArray = toTypedArray;
