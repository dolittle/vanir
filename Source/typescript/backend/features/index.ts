// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export * from './FeaturesProvider';

import { constructor } from '@dolittle/vanir-dependency-inversion';
import { IFeaturesProvider, initialize as initializeFeatures } from '@dolittle/vanir-features';
import { container } from 'tsyringe';
import { FeaturesProvider } from './FeaturesProvider';

export function initialize(): void {
    initializeFeatures();
    container.registerSingleton(IFeaturesProvider as constructor<IFeaturesProvider>, FeaturesProvider);
}
