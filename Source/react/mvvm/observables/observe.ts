// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { PropertyAccessor } from '@dolittle/types';

export function observe<TClass extends object = object>(...properties: PropertyAccessor<TClass>[]) {
    return function (target: TClass, propertyKey: string | symbol, descriptor: PropertyDescriptor) {
        if (typeof descriptor.value === 'function') {
        } else if (descriptor.get) {
        }
        return descriptor;
    };
}
