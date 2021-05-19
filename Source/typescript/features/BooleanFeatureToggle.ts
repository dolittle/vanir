// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggle } from './IFeatureToggle';

/**
 * Represents an implementation of {@link IFeatureToggle} for a simple true / false scenario.
 */
export class BooleanFeatureToggle implements IFeatureToggle {

    /**
     * Initializes a new instance of {@link BooleanFeatureToggle}.
     * @param {boolean} isOn Whether or not the feature is on.
     */
    constructor(readonly isOn: boolean) { }
}
