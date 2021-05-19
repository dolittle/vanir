// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * Defines a strategy for toggling a {@link Feature}
 */
export interface IFeatureToggle {
    /**
     * Get whether or not the feature is on.
     */
    readonly isOn: boolean;
}
