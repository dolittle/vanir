// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Binary } from 'mongodb';
import { Guid } from '@dolittle/rudiments';
import { CustomType } from './CustomType';
import './GuidExtensions';


/**
 * Represents an implementation of a {@link CustomType} for {@link Guid}.
 */
export class GuidCustomType extends CustomType<Guid> {

    constructor() {
        super(Guid);
    }

    /** @inheritdoc */
    toBSON(value: Guid): Binary {
        return value.toMUUID();
    }

    /** @inheritdoc */
    fromBSON(value: Binary): Guid {
        if (!value || !(value as any).toGuid) {
            return Guid.empty;
        }
        return value.toGuid();
    }
}
