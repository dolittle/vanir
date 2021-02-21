// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { CustomType } from './CustomType';

export class CustomTypesForTarget {
    [key: string]: CustomType;
}

/**
 * Represents a system for working with custom types
 */
export class CustomTypes {
    private static readonly _customTypesPerTargetType: Map<Constructor, CustomTypesForTarget> = new Map();

    /**
     * Register a custom type for a specific property on a type.
     * @param {Constructor} target Target type that holds the property.
     * @param {string} property Name of the property.
     * @param {Constructor<CustomType>} customType Type of custom type.
     */
    static register(target: Constructor, property: string, customType: Constructor<CustomType>) {
        let customTypes = {} as CustomTypesForTarget;
        if (this._customTypesPerTargetType.has(target)) {
            customTypes = this._customTypesPerTargetType.get(target)!;
        } else {
            this._customTypesPerTargetType.set(target, customTypes);
        }

        const serializer = new customType();
        serializer.targetType.prototype.toBSON = function (value: any) {
            return serializer.toBSON(this);
        };

        customTypes[property] = serializer;
    }

    /**
     * Check if there is a custom type map for a given type.
     * @param {Constructor} target Target type that holds the properties.
     */
    static hasFor(target: Constructor) {
        return this._customTypesPerTargetType.has(target);
    }

    /**
     * Get custom type map for a given type.
     * @param {Constructor} target Target type that holds the properties.
     */
    static getFor(target: Constructor) {
        return this._customTypesPerTargetType.get(target);
    }

    /**
     * Deserialize a property on a type.
     * @param {Constructor} target Target type that holds the properties.
     * @param {string} property Property to deserialize.
     * @param {any} value Value to deserialize.
     */
    static deserializeProperty(target: Constructor, property: string, value: any) {
        if (!value) {return value;}

        if (this.hasFor(target)) {
            const customTypes = this.getFor(target)!;
            if (customTypes.hasOwnProperty(property)) {
                const customType = customTypes[property];

                if (Array.isArray(value)) {
                    value = value.map(_ => customType.fromBSON(_));
                } else {
                    value = customType.fromBSON(value);
                }
            }
        }

        return value;
    }
}
