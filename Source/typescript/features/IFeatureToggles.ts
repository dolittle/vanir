// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * Defines an interface for checking if a feature is on or off.
 */
export abstract class IFeatureToggles {
    /**
     * Check if a feature is on or off.
     * @param {string} feature Feature identifier.
     * @returns True if on, false it off.
     * @notes If the feature is not configured, it will return false.
     */
    abstract isFeatureOn(feature: string): boolean;
}

