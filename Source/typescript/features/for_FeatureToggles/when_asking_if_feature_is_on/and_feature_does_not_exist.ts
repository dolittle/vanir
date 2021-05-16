// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeaturesProvider } from '../../IFeaturesProvider';
import sinon from 'sinon';
import { Features } from '../../Features';
import { FeatureToggles } from '../../FeatureToggles';

describe('when asking if feature is on and feature does not exist', () => {
    const provider: IFeaturesProvider = {
        provide: sinon.stub().returns(new Features())
    };

    const toggles = new FeatureToggles(provider);

    const result = toggles.isOn('SomeFeature');

    it('should return false', () => result.should.be.false);
});
