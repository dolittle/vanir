// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Extensions } from 'type-graphql';
import { getMetadataStorage } from 'type-graphql/dist/metadata';


/**
 * Decorator to use for setting route for a containing class of queries or mutations.
 * @param {string} path Path to use
 */
export function graphRoot(path: string): ClassDecorator & MethodDecorator;
export function graphRoot(path: string): ClassDecorator | MethodDecorator {
    return function (target: any, propertyKey: string | symbol, descriptor: TypedPropertyDescriptor<any>) {
        const storage = getMetadataStorage();
        if (!propertyKey) {
            const properties = Object.getOwnPropertyNames(target.prototype);
            for (const property of properties) {
                const existing = storage.fieldExtensions.find(_ => _.fieldName === property && _.target === target);

                let fullGraphRoot = '';

                if (existing && existing.extensions?.graphRootProperty) {
                    fullGraphRoot = `${path}/${existing.extensions?.graphRootProperty}`;
                } else {
                    fullGraphRoot = path;
                }

                storage.collectExtensionsFieldMetadata({
                    fieldName: property,
                    target,
                    extensions: {
                        graphRootClass: path,
                        graphRoot: fullGraphRoot
                    }
                });
            }
        }
        Extensions({ graphRootProperty: path })(target, propertyKey, descriptor);
    };
}
