// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeaturesProvider } from '../../IFeaturesProvider';
import { Features } from '../../Features';
import { FeatureToggles } from '../../FeatureToggles';
import { IFeatureToggleStrategy } from '../../IFeatureToggleStrategy';
import { BehaviorSubject } from 'rxjs';

describe('when asking if feature is on and feature exists and strategy is turned off', () => {
    const feature = 'SomeFeature';
    const strategy: IFeatureToggleStrategy = {
        get isOn(): boolean {
            return false;
        }
    };

    const provider: IFeaturesProvider = {
        features: new BehaviorSubject(new Features(new Map([
            [feature, strategy]
        ])))
    };

    const toggles = new FeatureToggles(provider);

    const result = toggles.isOn(feature);

    it('should return false', () => result.should.be.false);
});
