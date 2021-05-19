// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggle } from './IFeatureToggle';

/**
 * Represents a feature in the system.
 */
export class Feature {

    /**
     * Initializes a new instance of {@link Feature}.
     * @param {string} name Name of the feature.
     * @param {string} description Description of the feature.
     * @param {IFeatureToggle[]} toggles Toggles for the feature.
     */
    constructor(readonly name: string, readonly description: string, readonly toggles: IFeatureToggle[]) {
    }

    /**
     * Gets whether feature is on or off.
     */
    get isOn() {
        return this.toggles.some(_ => _.isOn);
    }
}
