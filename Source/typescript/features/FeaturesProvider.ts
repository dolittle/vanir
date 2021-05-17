// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import fs from 'fs';

import { Features } from './Features';
import { IFeaturesProvider } from './IFeaturesProvider';
import { IFeatureToggleStrategy } from './IFeatureToggleStrategy';
import { BooleanFeatureToggleStrategy } from './BooleanFeatureToggleStrategy';
import { injectable } from 'tsyringe';

const featuresPath = './data/features.json';

/**
 * Represents an implementation of {@link IFeaturesProvider}.
 */
 @injectable()
export class FeaturesProvider extends IFeaturesProvider {

    /** @inheritdoc */
    provide(): Features {
        if (!fs.existsSync(featuresPath)) return new Features();
        const featuresAsJson = fs.readFileSync(featuresPath).toString();
        const featuresBooleans = JSON.parse(featuresAsJson);
        const features = new Map<string, IFeatureToggleStrategy>();
        for (const feature in featuresBooleans) {
            features.set(feature, new BooleanFeatureToggleStrategy(featuresBooleans[feature]));
        }
        return new Features(features);
    }
}
