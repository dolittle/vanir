// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable, container, singleton } from 'tsyringe';
import { PubSubEngine, Query, Resolver, Root, Subscription } from 'type-graphql';
import { ILogger } from '../logging';
import { Feature } from './Feature';
import { FeatureNotification } from './FeatureNotification';
import { IFeaturesProvider, Features } from '@dolittle/vanir-features';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { FeaturesProvider } from './FeaturesProvider';


@injectable()
@singleton()
@Resolver()
export class FeaturesSubscriptionsResolver {

    static initialize() {
        let featuresProvider = container.resolve(IFeaturesProvider as constructor<IFeaturesProvider>);
        let pubSub = container.resolve(PubSubEngine as constructor<PubSubEngine>);

        featuresProvider.features.subscribe(_ => {
            pubSub.publish('FEATURES', {});
        });
    }

    private _features!: Features;

    constructor(private readonly featuresProvider: FeaturesProvider) {
        featuresProvider.features.subscribe(_ => this._features = _);
    }

    @Query(returns => FeatureNotification)
    features(): FeatureNotification {
        return this.getNotification();
    }


    @Subscription(returns => FeatureNotification, {
        topics: 'FEATURES'
    })
    newFeatures(@Root() notification: any): FeatureNotification {
        return this.getNotification();
    }

    private getNotification() {
        const message = new FeatureNotification();
        this._features.forEach((value, key, map) => {
            const feature = new Feature();
            feature.name = key;
            feature.isOn = value.isOn;
            message.features.push(feature);
        });
        return message;
    }
}
