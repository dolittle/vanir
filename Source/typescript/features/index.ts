// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


export * from './BooleanFeatureToggle';
export * from './featureDecorator';
export * from './FeatureDecorators';
export * from './Feature';
export * from './FeaturesParser';
export * from './FeatureDefinitions';
export * from './IFeatureDefinition';
export * from './IFeatureToggleDefinition';
export * from './Features';
export * from './FeatureToggles';
export * from './IFeaturesParser';
export * from './IFeaturesProvider';
export * from './IFeatureToggles';
export * from './IFeatureToggle';
import { container } from 'tsyringe';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { IFeatureToggles } from './IFeatureToggles';
import { FeatureToggles } from './FeatureToggles';
import { IFeaturesParser } from './IFeaturesParser';
import { FeaturesParser } from './FeaturesParser';

export function initialize(): void {
    container.registerSingleton(IFeatureToggles as constructor<IFeatureToggles>, FeatureToggles);
    container.registerSingleton(IFeaturesParser as constructor<IFeaturesParser>, FeaturesParser);
}
