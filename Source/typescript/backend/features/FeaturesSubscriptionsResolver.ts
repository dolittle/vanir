// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable, container, singleton } from 'tsyringe';
import { PubSubEngine, Query, Resolver, Root, Subscription } from 'type-graphql';
import { FeatureDefinition } from './FeatureDefinition';
import { FeatureNotification } from './FeatureNotification';
import { IFeaturesProvider, Features } from '@dolittle/vanir-features';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { graphRoot } from '../graphql/graphRootDecorator';
import { FeatureToggleDefinition } from './FeatureToggleDefinition';

@injectable()
@singleton()
@Resolver()
@graphRoot('system')
export class FeaturesSubscriptionsResolver {

    static initialize() {
        const featuresProvider = container.resolve(IFeaturesProvider as constructor<IFeaturesProvider>);
        const pubSub = container.resolve(PubSubEngine as constructor<PubSubEngine>);

        featuresProvider.features.subscribe(_ => {
            pubSub.publish('FEATURES', {});
        });
    }

    private _features!: Features;

    constructor(private readonly featuresProvider: IFeaturesProvider) {
        featuresProvider.features.subscribe(_ => this._features = _);
    }

    @Query(returns => FeatureNotification)
    features(): FeatureNotification {
        return this.getNotification();
    }


    @Subscription(returns => FeatureNotification, {
        topics: 'FEATURES'
    })
    system_newFeatures(@Root() notification: any): FeatureNotification {
        return this.getNotification();
    }

    private getNotification() {
        const message = new FeatureNotification();
        this._features.forEach((value, key, map) => {
            const feature = new FeatureDefinition();
            feature.name = key;
            feature.description = value.description || '';
            feature.toggles = value.toggles.map(_ => {
                const toggleDefinition = new FeatureToggleDefinition()
                toggleDefinition.type = 'Boolean';
                toggleDefinition.isOn = value.isOn;
                return toggleDefinition;
            })
            message.features.push(feature);
        });
        return message;
    }
}
