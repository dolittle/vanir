// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import fs from 'fs';

import { Feature, Features, IFeaturesProvider } from '@dolittle/vanir-features';
import { BooleanFeatureToggle } from '@dolittle/vanir-features';
import { injectable } from 'tsyringe';
import { Observable, BehaviorSubject } from 'rxjs';

import chokidar from 'chokidar';
import { FeatureDefinition } from './FeatureDefinition';

const featuresPath = './data/features.json';

type FeatureDefinitions = { [key: string]: FeatureDefinition };

/**
 * Represents an implementation of {@link IFeaturesProvider}.
 */
@injectable()
export class FeaturesProvider extends IFeaturesProvider {
    readonly _features: BehaviorSubject<Features> = new BehaviorSubject(new Features());

    constructor() {
        super();
        chokidar.watch(featuresPath).on('all', (event, path) => {
            this.loadFeatures();
        });
        this.loadFeatures();
    }

    /** @inheritdoc */
    get features(): Observable<Features> {
        return this._features;
    }

    /** @inheritdoc */
    private loadFeatures(): void {
        if (!fs.existsSync(featuresPath)) {
            this._features.next(new Features());
            return;
        }

        const featuresAsJson = fs.readFileSync(featuresPath).toString();
        const featuresDefinitions = JSON.parse(featuresAsJson) as FeatureDefinitions;
        const features = new Map<string, Feature>();
        for (const featureName in featuresDefinitions) {
            const featureDefinition = featuresDefinitions[featureName];

            features.set(featureName, new Feature(
                featureName,
                featureDefinition.description,
                featureDefinition.toggles.map(_ => new BooleanFeatureToggle(_.isOn))));
        }
        this._features.next(new Features(features));
    }
}
