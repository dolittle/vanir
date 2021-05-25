// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';

export class ClassAndMethod {
    constructor(readonly target: Constructor, readonly method: string) { }
}

/**
 * Represents all the feature decorators in the system.
 */
export class FeatureDecorators {
    static readonly classFeatures: Map<Constructor, string[]> = new Map();
    static readonly methodFeatures: Map<ClassAndMethod, string[]> = new Map();

    static registerForClass(target: Constructor, feature: string) {
        if (!this.classFeatures.has(target)) {
            this.classFeatures.set(target, []);
        }

        this.classFeatures.set(target, [...this.classFeatures.get(target)!, ...[feature]]);
    }

    static registerForMethod(target: Constructor, method: string, feature: string) {
        let existing: ClassAndMethod | undefined;
        this.methodFeatures.forEach((value, key) => {
            if (key.target === target && key.method === method) {
                existing = key;
            }
        });

        if (!existing) {
            existing = new ClassAndMethod(target, method);
        }

        if (!this.methodFeatures.has(existing)) {
            this.methodFeatures.set(existing, []);
        }

        this.methodFeatures.set(existing, [...this.methodFeatures.get(existing)!, ...[feature]]);
    }
}
