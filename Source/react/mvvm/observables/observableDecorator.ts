// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { ObservableProperties } from './ObservableProperties';

export function observable(target: any, key: string) {
    ObservableProperties.registerPropertyForType(target.__proto__, key);
}


