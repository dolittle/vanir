// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { Cursor } from 'mongodb';
import { CustomTypes } from './CustomTypes';

declare module 'mongodb' {
    interface Cursor {
        toTypedArray<T>(type: Constructor<T>): Promise<T[]>;
    }
}

/**
 * Convert cursor to a typed array with a given element type.
 * @param {Cursor} this Cursor.
 * @param {Constructor<T>} type Type of array element.
 */
async function toTypedArray<T>(this: Cursor, type: Constructor<T>): Promise<T[]> {
    const result = await this.toArray();
    const mappedResult = result.map(_ => {
        const instance = new type();

        for (const property in _) {
            (instance as any)[property] = CustomTypes.deserializeProperty(type, property, _[property]);
        }

        return instance;
    });

    return mappedResult;
}

Cursor.prototype.toTypedArray = toTypedArray;
