// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FeaturesParser } from '../FeaturesParser';

const first_feature = 'my.first.feature';
const second_feature = 'my.second.feature';
const first_feature_description = 'My first feature';
const second_feature_description = 'My second feature';



const json: string =
    `{` +
    `   "${first_feature}": {` +
    `       "description": "${first_feature_description}",` +
    `       "toggles": [{` +
    `           "type": "Boolean",` +
    `           "isOn": false` +
    `       }]` +
    `   },` +
    `   "${second_feature}": {` +
    `       "description": "${second_feature_description}",` +
    `       "toggles": [{` +
    `           "type": "Boolean",` +
    `           "isOn": true` +
    `       }]` +
    `   }` +
    `}`;



describe('when parsing with two features with one toggle each', () => {

    const parser = new FeaturesParser();

    const result = parser.parse(json);

    it('should contain first feature', () => result.has(first_feature).should.be.true);
    it('should hold first features description', () => result.get(first_feature)?.description.should.equal(first_feature_description));
    it('should have one toggle for first feature', () => result.get(first_feature)?.toggles.length.should.equal(1));
    it('should have toggle for first feature set to false', () => result.get(first_feature)?.toggles[0].isOn.should.be.false);
    it('should contain second feature', () => result.has(second_feature).should.be.true);
    it('should hold second features description', () => result.get(second_feature)?.description.should.equal(second_feature_description));
    it('should have one toggle for second feature', () => result.get(second_feature)?.toggles.length.should.equal(1));
    it('should have toggle for second feature set to true', () => result.get(second_feature)?.toggles[0].isOn.should.be.true);
});
