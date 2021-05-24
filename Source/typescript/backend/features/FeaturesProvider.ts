// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import fs from 'fs';

import { Feature, Features, IFeaturesProvider } from '@dolittle/vanir-features';
import { BooleanFeatureToggle } from '@dolittle/vanir-features';
import { injectable } from 'tsyringe';
import { Observable, BehaviorSubject } from 'rxjs';

import chokidar from 'chokidar';
import { FeatureDefinition } from './FeatureDefinition';
import { IFeaturesParser } from '../../features/IFeaturesParser';

const featuresPath = './data/features.json';

/**
 * Represents an implementation of {@link IFeaturesProvider}.
 */
@injectable()
export class FeaturesProvider extends IFeaturesProvider {
    readonly _features: BehaviorSubject<Features> = new BehaviorSubject(new Features());

    constructor(private readonly _parser: IFeaturesParser) {
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
        const features = this._parser.parse(featuresAsJson);
        this._features.next(features);
    }
}
