// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Feature } from '../Feature';
import { Features } from '../Features';
import { BooleanFeatureToggle } from '../BooleanFeatureToggle';
import { FeatureDefinitions } from '../FeatureDefinitions';


const first_feature = 'my.first.feature';
const second_feature = 'my.second.feature';
const first_feature_description = 'My first feature';
const second_feature_description = 'My second feature';


const featuresMap = new Map<string, Feature>();

featuresMap.set(first_feature, new Feature(first_feature, first_feature_description, [new BooleanFeatureToggle(false)]));
featuresMap.set(second_feature, new Feature(second_feature, second_feature_description, [new BooleanFeatureToggle(true)]));
const features = new Features(featuresMap);

describe('when converting to json', () => {

    const json = features.toJSON();
    const definitions = JSON.parse(json) as FeatureDefinitions;

    it('should contain first feature', () => definitions.hasOwnProperty(first_feature).should.be.true);
    it('should contain first features description', () => definitions[first_feature].description.should.equal(first_feature_description));
    it('should have one toggle for first feature', () => definitions[first_feature].toggles.length.should.equal(1));
    it('should have first features toggle set to off', () => definitions[first_feature].toggles[0].isOn.should.equal(false));
    it('should contain second feature', () => definitions.hasOwnProperty(second_feature).should.be.true);
    it('should contain second features description', () => definitions[second_feature].description.should.equal(second_feature_description));
    it('should have one toggle for second feature', () => definitions[second_feature].toggles.length.should.equal(1));
    it('should have second features toggle set to off', () => definitions[second_feature].toggles[0].isOn.should.equal(true));
});
