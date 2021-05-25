// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Features } from './Features';

/**
 * Defines a system that can parse features from JSON.
 */
export abstract class IFeaturesParser {

    /**
     * Parse a JSON representation.
     * @param {string} json JSON as string to parse.
     * @returns {Features} instance.
     */
    abstract parse(json: string): Features;
}
