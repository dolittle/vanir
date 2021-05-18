// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { gql } from '@apollo/client';
import { Features, IFeaturesProvider, IFeatureToggleStrategy, BooleanFeatureToggleStrategy } from '@dolittle/vanir-features';
import { BehaviorSubject, Observable } from 'rxjs';
import { injectable } from 'tsyringe';
import { DataSource } from '../DataSource';
import { Subscriptions } from '../Subscriptions';

export type Feature = {
    name: string;
    isOn: boolean;
};

export type FeatureNotification = {
    features: Feature[];
};

/**
 * Represents an implementation of {@link IFeaturesProvider}.
 */
@injectable()
export class FeaturesProvider extends IFeaturesProvider {
    readonly _features: BehaviorSubject<Features> = new BehaviorSubject(new Features());

    constructor(
        private readonly _dataSource: DataSource,
        private readonly _subscriptions: Subscriptions) {
        super();

        this.populateInitial();
        this.setupSubscription(_subscriptions);
    }

    /** @inheritdoc */
    get features(): Observable<Features> {
        return this._features;
    }

    private async populateInitial() {
        const query = gql`
            query {
                system {
                    features {
                        features {
                            name
                            isOn
                        }
                    }
                }
            }
        `;

        const result = await this._dataSource.query({ query });
        this.setFeaturesFromNotification(result.data.system.features);
    }


    private setupSubscription(_subscriptions: Subscriptions) {
        const query = gql`
            subscription {
                system_newFeatures {
                    features {
                        name
                        isOn
                    }
                }
            }`;
        _subscriptions.subscribe({
            query
        }).subscribe(r => {
            this.setFeaturesFromNotification(r.data.system_newFeatures);
        });
    }

    private setFeaturesFromNotification(notification: FeatureNotification) {
        const featureMap = new Map<string, IFeatureToggleStrategy>();
        for (const feature of notification.features) {
            featureMap.set(feature.name, new BooleanFeatureToggleStrategy(feature.isOn));
        }
        this._features.next(new Features(featureMap));
    }
}

