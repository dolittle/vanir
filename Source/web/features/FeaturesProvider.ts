// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Features, IFeaturesProvider } from '@dolittle/vanir-features';
import { BehaviorSubject, Observable } from 'rxjs';
import { injectable } from 'tsyringe';

/**
 * Represents an implementation of {@link IFeaturesProvider}.
 */
@injectable()
export class FeaturesProvider extends IFeaturesProvider {
    readonly _features: BehaviorSubject<Features> = new BehaviorSubject(new Features());

    constructor() {
        super();
    }

    /** @inheritdoc */
    get features(): Observable<Features> {
        return this._features;
    }
}

