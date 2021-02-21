// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { customType } from './index';
import { GuidCustomType } from './GuidCustomType';

/**
 * Decorator for specifying a custom type supporting {@link Guid}.
 */
export function guid() {
    return customType(GuidCustomType);
}
