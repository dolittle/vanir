// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { graphRoot } from '@dolittle/vanir-backend';
import { Query } from 'type-graphql';

@graphRoot('cart')
export class Queries {
    @Query(returns => Number)
    content(): number {
        return 42;
    }
}