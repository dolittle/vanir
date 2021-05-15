// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggleStrategy } from './IFeatureToggleStrategy';

/**
 * Represents an implementation of {@link IFeatureToggleStrategy} for a simple true / false scenario.
 */
export class BooleanFeatureToggleStrategy implements IFeatureToggleStrategy {

    /**
     * Initializes a new instance of {@link BooleanFeatureToggleStrategy}.
     * @param {boolean} isOn Whether or not the feature is on.
     */
    constructor(readonly isOn: boolean) { }
}
