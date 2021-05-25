// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FeatureDecorators } from './FeatureDecorators';

export function feature(name: string): ClassDecorator & MethodDecorator;
export function feature(name: string): ClassDecorator | MethodDecorator {
    return function (target: any, propertyKey: string | symbol, descriptor: TypedPropertyDescriptor<any>) {
        if (!propertyKey) {
            FeatureDecorators.registerForClass(target, name);
        } else {
            FeatureDecorators.registerForMethod(target.constructor, propertyKey as string, name);
        }
    };
}
