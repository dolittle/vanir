// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import {
    OperationVariables,
    SubscriptionOptions,
    Observable,
    FetchResult
} from '@apollo/client';

export abstract class Subscriptions {
    abstract subscribe<T = any, TVariables = OperationVariables>(options: SubscriptionOptions<TVariables>): Observable<FetchResult<T>>;

}
