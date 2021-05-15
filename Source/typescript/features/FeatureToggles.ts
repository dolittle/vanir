// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggles } from './IFeatureToggles';

/**
 * Represents an implementation of {@link IFeatureToggles}
 */

export class FeatureToggles implements IFeatureToggles {

    /** @inheritdoc */
    isOn(feature: string): boolean {
        throw new Error('Method not implemented.');
    }
}
