// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeaturesProvider } from '../../IFeaturesProvider';
import { Features } from '../../Features';
import { FeatureToggles } from '../../FeatureToggles';
import { IFeatureToggle } from '../../IFeatureToggle';
import { BehaviorSubject } from 'rxjs';
import { Feature } from '../../Feature';

describe('when asking if feature is on and feature exists and strategy is turned off', () => {
    const featureName = 'SomeFeature';

    const strategy: IFeatureToggle = {
        get isOn(): boolean {
            return false;
        }
    };
    const feature = new Feature(featureName, '', [strategy]);

    const provider: IFeaturesProvider = {
        features: new BehaviorSubject(new Features(new Map([
            [featureName, feature]
        ])))
    };

    const toggles = new FeatureToggles(provider);

    const result = toggles.isOn(featureName);

    it('should return false', () => result.should.be.false);
});
