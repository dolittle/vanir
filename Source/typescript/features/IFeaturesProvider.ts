// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Features } from './Features';

import { Observable } from 'rxjs';

/**
 * Defines a system that can provide {@link Features}
 */
export abstract class IFeaturesProvider {

    /**
     * Features {@link Observable} providing {@link Features}.
     */
    abstract readonly features: Observable<Features>;
}
