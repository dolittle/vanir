// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Feature } from './Feature';

/**
 * Represents a set of {@link Features}.
 */
export class Features implements ReadonlyMap<string, Feature> {
    private readonly _features: Map<string, Feature>;

    /**
     * Initializes a new instance of {@link Features}.
     * @param {Map<string, Feature>} [features] Optional map of features.
     */
    constructor(features?: Map<string, Feature>) {
        this._features = features || new Map<string, Feature>();
    }

    /** @inheritdoc */
    forEach(callbackfn: (value: Feature, key: string, map: ReadonlyMap<string, Feature>) => void, thisArg?: any): void {
        this._features.forEach(callbackfn);
    }

    /** @inheritdoc */
    get(key: string): Feature | undefined {
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
    [Symbol.iterator](): IterableIterator<[string, Feature]> {
        return this._features[Symbol.iterator]();
    }

    /** @inheritdoc */
    entries(): IterableIterator<[string, Feature]> {
        return this._features.entries();
    }

    /** @inheritdoc */
    keys(): IterableIterator<string> {
        return this._features.keys();
    }

    /** @inheritdoc */
    values(): IterableIterator<Feature> {
        return this._features.values();
    }
}
