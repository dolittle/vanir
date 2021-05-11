// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export function feature(name: string): ClassDecorator & MethodDecorator;
export function feature(name: string): ClassDecorator | MethodDecorator {
    return function (target: any, propertyKey: string | symbol, descriptor: TypedPropertyDescriptor<any>) {
    }
}


