// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import fs from 'fs';

import { Features } from './Features';
import { IFeaturesProvider } from './IFeaturesProvider';
import { IFeatureToggleStrategy } from './IFeatureToggleStrategy';
import { BooleanFeatureToggleStrategy } from './BooleanFeatureToggleStrategy';
import { injectable } from 'tsyringe';
import { Observable, BehaviorSubject } from 'rxjs';

const featuresPath = './data/features.json';

/**
 * Represents an implementation of {@link IFeaturesProvider}.
 */
@injectable()
export class FeaturesProvider extends IFeaturesProvider {
    readonly _features: BehaviorSubject<Features> = new BehaviorSubject(new Features());

    constructor() {
        super();
        fs.watchFile(featuresPath, _ => {
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
        const featuresBooleans = JSON.parse(featuresAsJson);
        const features = new Map<string, IFeatureToggleStrategy>();
        for (const feature in featuresBooleans) {
            features.set(feature, new BooleanFeatureToggleStrategy(featuresBooleans[feature]));
        }
        this._features.next(new Features(features));
    }
}
