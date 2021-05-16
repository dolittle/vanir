// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeaturesProvider } from '../../IFeaturesProvider';
import sinon from 'sinon';
import { Features } from '../../Features';
import { FeatureToggles } from '../../FeatureToggles';
import { IFeatureToggleStrategy } from '../../dist/IFeatureToggleStrategy';

describe('when asking if feature is on and feature exists and strategy is turned on', () => {
    const feature = 'SomeFeature';
    const strategy: IFeatureToggleStrategy = {
        get isOn(): boolean {
            return true;
        }
    };

    const provider: IFeaturesProvider = {
        provide: sinon.stub().returns(new Features(new Map([
            [feature, strategy]
        ])))
    };

    const toggles = new FeatureToggles(provider);

    const result = toggles.isOn(feature);

    it('should return true', () => result.should.be.true);
});
