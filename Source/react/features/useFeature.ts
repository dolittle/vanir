// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import { IFeatureToggles, IFeaturesProvider } from '@dolittle/vanir-features';
import { container } from 'tsyringe';
import { constructor } from '@dolittle/vanir-dependency-inversion';

let featureToggles: IFeatureToggles;
let featureProvider: IFeaturesProvider;

export const useFeature = (name: string) => {
    if (!featureToggles) {
        featureToggles = container.resolve(IFeatureToggles as constructor<IFeatureToggles>);
    }
    if (!featureProvider) {
        featureProvider = container.resolve(IFeaturesProvider as constructor<IFeaturesProvider>);
    }
    const [isOn, setIsOn] = useState(featureToggles.isOn(name));

    useEffect(() => {
        const subscription = featureProvider.features.subscribe(_ => {
            setIsOn(featureToggles.isOn(name));
        });

        return () => subscription.unsubscribe();
    });

    return isOn;
};
