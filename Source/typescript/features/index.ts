// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


export * from './BooleanFeatureToggleStrategy';
export * from './featureDecorator';
export * from './FeatureDecorators';
export * from './Features';
export * from './FeaturesProvider';
export * from './FeatureToggles';
export * from './IFeaturesProvider';
export * from './IFeatureToggles';
export * from './IFeatureToggleStrategy';
import { container } from 'tsyringe';
import { IFeaturesProvider } from './IFeaturesProvider';
import { FeaturesProvider } from './FeaturesProvider';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { IFeatureToggles } from './IFeatureToggles';
import { FeatureToggles } from './FeatureToggles';

export function initialize(): void {
    container.registerSingleton(IFeaturesProvider as constructor<IFeaturesProvider>, FeaturesProvider);
    container.registerSingleton(IFeatureToggles as constructor<IFeatureToggles>, FeatureToggles);
}
