// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ApolloClient, HttpLink, InMemoryCache } from '@apollo/client';
import { WebSocketLink } from '@apollo/client/link/ws';
import { container } from 'tsyringe';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { DataSource } from './DataSource';
import { Configuration } from './Configuration';
import { INavigator, Navigator } from './routing';
import { Bindings as MessagingBindings } from './messaging/Bindings';
import { Bindings as FeaturesBindings } from './features';
import { Subscriptions } from './Subscriptions';

export class Bindings {
    static initialize(configuration: Configuration) {
        const cache = new InMemoryCache();
        const link = new HttpLink({
            uri: `${configuration.prefix}/graphql/`
        });

        const dataClient = new ApolloClient({
            cache,
            link,
            name: `${configuration.name} Client`,
            version: configuration.versionInfo.version,
            queryDeduplication: false,
            defaultOptions: {
                mutate: {
                    fetchPolicy: 'no-cache'
                },
                query: {
                    fetchPolicy: 'no-cache'
                },
                watchQuery: {
                    fetchPolicy: 'no-cache'
                }
            }
        });
        container.registerInstance(DataSource as constructor<DataSource>, dataClient);

        const subscriptionLink = new WebSocketLink({
            uri: `ws://${window.location.host}${configuration.prefix}/graphql`,
            options: {
                reconnect: true
            }
        });

        const subscriptionsClient = new ApolloClient({
            cache,
            link: subscriptionLink,
            name: `${configuration.name} WS Client`,
            version: configuration.versionInfo.version,
            queryDeduplication: false,
            defaultOptions: {
                mutate: {
                    fetchPolicy: 'no-cache'
                },
                query: {
                    fetchPolicy: 'no-cache'
                },
                watchQuery: {
                    fetchPolicy: 'no-cache'
                }
            }
        });
        container.registerInstance(Subscriptions as constructor<Subscriptions>, subscriptionsClient);

        container.registerSingleton(INavigator as constructor<INavigator>, Navigator);
        container.registerInstance(History, window.history);

        MessagingBindings.initialize();
        FeaturesBindings.initialize();
    }
}
