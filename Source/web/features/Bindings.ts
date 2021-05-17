// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { constructor } from '@dolittle/vanir-dependency-inversion';
import { IFeaturesProvider, initialize as initializeFeatures } from '@dolittle/vanir-features';
import { container } from 'tsyringe';
import { FeaturesProvider } from './FeaturesProvider';

export class Bindings {
    static initialize() {
        initializeFeatures();
        container.registerSingleton(IFeaturesProvider as constructor<IFeaturesProvider>, FeaturesProvider);
    }
}
