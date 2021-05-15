// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Features } from './Features';

/**
 * Defines a system that can provide {@link Features}
 */
export abstract class IFeaturesProvider {

    /**
     * Provide {@link Features}.
     * @returns All {@link Features}.
     */
    abstract provide(): Features;
}



