// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggleStrategy } from './IFeatureToggleStrategy';

/**
 * Represents a set of {@link Features}.
 */
export class Features implements ReadonlyMap<string, IFeatureToggleStrategy> {
    private readonly _features: Map<string, IFeatureToggleStrategy>;

    /**
     * Initializes a new instance of {@link Features}.
     * @param {Map<string, IFeatureToggleStrategy>} [features] Optional map of features.
     */
    constructor(features?: Map<string, IFeatureToggleStrategy>) {
        this._features = features || new Map<string, IFeatureToggleStrategy>();
    }

    /** @inheritdoc */
    forEach(callbackfn: (value: IFeatureToggleStrategy, key: string, map: ReadonlyMap<string, IFeatureToggleStrategy>) => void, thisArg?: any): void {
        this._features.forEach(callbackfn);
    }

    /** @inheritdoc */
    get(key: string): IFeatureToggleStrategy | undefined {
        return this._features.get(key);
    }

    /** @inheritdoc */
    has(key: string): boolean {
        return this._features.has(key);
    }

    /** @inheritdoc */
    get size(): number {
        return this._features.size;
    }

    /** @inheritdoc */
    [Symbol.iterator](): IterableIterator<[string, IFeatureToggleStrategy]> {
        return this._features[Symbol.iterator]();
    }

    /** @inheritdoc */
    entries(): IterableIterator<[string, IFeatureToggleStrategy]> {
        return this._features.entries();
    }

    /** @inheritdoc */
    keys(): IterableIterator<string> {
        return this._features.keys();
    }

    /** @inheritdoc */
    values(): IterableIterator<IFeatureToggleStrategy> {
        return this._features.values();
    }
}
