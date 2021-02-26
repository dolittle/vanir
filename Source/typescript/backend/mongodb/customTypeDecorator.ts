// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { CustomType } from './CustomType';
import { CustomTypes } from './CustomTypes';

/**
 * Decorator for telling what {@link CustomType} to use for serialization - supports a single item and arrays
 * @param {Constructor<CustomType>} customTypeType Type of custom type to use.
 */
export function customType(customTypeType: Constructor<CustomType>) {
    return function (target: any, propertyKey: string, descriptor?: PropertyDescriptor) {
        CustomTypes.register(target.constructor, propertyKey, customTypeType);
    };
}
