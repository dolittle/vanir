// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';

/**
 * Represents all the feature decorators in the system.
 */
export class FeatureDecorators {
    private static readonly _classDecorators: Map<Constructor, string[]> = new Map();
    private static readonly _methodDecorators: Map<Function, string[]> = new Map();

    static registerForClass(target: Constructor, feature: string) {
        if (!this._classDecorators.has(target)) {
            this._classDecorators.set(target, []);
        }

        this._classDecorators.set(target, [...this._classDecorators.get(target)!, ...[feature]]);
    }

    static registerForMethod(target: Function, feature: string) {
        if (!this._methodDecorators.has(target)) {
            this._methodDecorators.set(target, []);
        }

        this._methodDecorators.set(target, [...this._methodDecorators.get(target)!, ...[feature]]);
    }

    static getForClass(target: Constructor): string[] {
        return this._classDecorators.get(target) || [];
    }

    static getForMethod(target: Function): string[] {
        return this._methodDecorators.get(target) || [];
    }
}
