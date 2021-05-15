// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Features } from './Features';
import { IFeaturesProvider } from './IFeaturesProvider';

/**
 * Represents an implementation of {@link IFeaturesProvider}.
 */
export class FeaturesProvider extends IFeaturesProvider {

    /** @inheritdoc */
    provide(): Features {
        throw new Error('Method not implemented.');
    }
}
