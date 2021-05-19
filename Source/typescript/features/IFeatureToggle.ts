// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * Defines a toggle for a feature.
 */
export interface IFeatureToggle {
    /**
     * Get whether or not the feature is on.
     */
    readonly isOn: boolean;
}
