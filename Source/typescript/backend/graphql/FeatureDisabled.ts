// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * Exception that is thrown when a feature is disabled.
 */
export class FeatureDisabled extends Error {
    constructor(feature: string) {
        super(`Feature '${feature}' is disabled`);
    }
}
