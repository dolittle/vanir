// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { BooleanFeatureToggle } from './BooleanFeatureToggle';
import { Feature } from './Feature';
import { Features } from './Features';
import { IFeatureDefinition } from './IFeatureDefinition';
import { IFeaturesParser } from './IFeaturesParser';

type FeatureDefinitions = { [key: string]: IFeatureDefinition };

/**
 * Represents an implementation of {@link IFeaturesParser}.
 */
export class FeaturesParser implements IFeaturesParser {

    /** @inheritdoc */
    parse(json: string): Features {
        const featuresDefinitions = JSON.parse(json) as FeatureDefinitions;
        const features = new Map<string, Feature>();
        for (const featureName in featuresDefinitions) {
            const featureDefinition = featuresDefinitions[featureName];

            features.set(featureName, new Feature(
                featureName,
                featureDefinition.description,
                featureDefinition.toggles.map(_ => new BooleanFeatureToggle(_.isOn))));
        }

        return new Features(features);
    }
}
