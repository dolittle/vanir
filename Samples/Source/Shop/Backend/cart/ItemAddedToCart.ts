// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { eventType } from '@dolittle/sdk.events';

@eventType('5ff11df2-68e6-4890-8cae-b22fd1e3c8b5')
export class ItemAddedToCart {
    constructor(readonly something: number) { }
}
