// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Feature } from './Feature';
import { IFeatureDefinition } from './IFeatureDefinition';
import { IFeatureToggleDefinition } from './IFeatureToggleDefinition';

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

    /**
     * Convert {@link Features} to a string JSON representation.
     * @returns {string}
     */
    toJSON() {
        const container: any = {};
        const definitions = this.toDefinitions();
        for (const definition of definitions) {
            container[definition.name] = definition;
            (definition as any).name = undefined;
            delete (definition as any).name;
        }
        return JSON.stringify(definitions, null, 2);
    }

    /**
     * Convert {@link Features} to a collection of {@link IFeatureDefinition}.
     * @returns {IFeatureDefinition[]}
     */
    toDefinitions(): IFeatureDefinition[] {
        return Array.from(this._features.values()).map(_ => {
            return {
                name: _.name,
                description: _.description,
                toggles: _.toggles.map(t => {
                    return {
                        type: 'Boolean',
                        isOn: _.isOn
                    } as IFeatureToggleDefinition;
                })
            } as IFeatureDefinition;
        });
    }
}
