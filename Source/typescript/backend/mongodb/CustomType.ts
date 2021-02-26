// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { Binary } from 'mongodb';

/**
 * Represents a custom type that is able to serialize and deserialize to and from mongo for a specific type.
 */

export abstract class CustomType<T = any> {
    constructor(readonly targetType: Constructor<T>) {
    }

    /**
     * Convert a value to a BSON binary.
     * @param {T} value Value to convert to binary.
     */
    abstract toBSON(value: T): Binary;

    /**
     * Convert from BSON binary to the target type.
     * @param {Binary} value Binary value to convert from.
     */
    abstract fromBSON(value: Binary): T;
}
