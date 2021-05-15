// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggles } from './IFeatureToggles';
import { IFeaturesProvider } from './IFeaturesProvider';
import { Features } from './Features';

/**
 * Represents an implementation of {@link IFeatureToggles}
 */
export class FeatureToggles implements IFeatureToggles {
    private readonly _features: Features;

    /**
     *
     * @param {IFeaturesProvider} provider Provider of features.
     */
    constructor(private readonly provider: IFeaturesProvider) {
        this._features = provider.provide();
    }

    /** @inheritdoc */
    isOn(feature: string): boolean {
        return this._features.get(feature)?.isOn || false;
    }
}
