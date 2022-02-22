// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Guid } from '@dolittle/rudiments';
import { MUUID, from as parseMUUID } from 'uuid-mongodb';
import { Binary } from 'mongodb';

declare module '@dolittle/rudiments' {
    interface Guid {
        /**
         * Converts a {@link Guid} to a {@link MUUID}
         */
        toMUUID(): MUUID;

        equals(other: any): boolean;
    }
}

Guid.prototype.toMUUID = function (): MUUID {
    const rearrangedBytes = [...this.bytes];
    rearrangedBytes[0] = this.bytes[3];
    rearrangedBytes[1] = this.bytes[2];
    rearrangedBytes[2] = this.bytes[1];
    rearrangedBytes[3] = this.bytes[0];

    rearrangedBytes[4] = this.bytes[5];
    rearrangedBytes[5] = this.bytes[4];

    rearrangedBytes[6] = this.bytes[7];
    rearrangedBytes[7] = this.bytes[6];

    const uuid = new Binary(Buffer.from(rearrangedBytes as number[]), Binary.SUBTYPE_UUID);
    return parseMUUID(uuid);
};

export function binaryToGuid(binary: Binary): Guid {
    const rearrangedBytes = [...binary.buffer];

    rearrangedBytes[0] = binary.buffer[3];
    rearrangedBytes[1] = binary.buffer[2];
    rearrangedBytes[2] = binary.buffer[1];
    rearrangedBytes[3] = binary.buffer[0];

    rearrangedBytes[4] = binary.buffer[5];
    rearrangedBytes[5] = binary.buffer[4];

    rearrangedBytes[6] = binary.buffer[7];
    rearrangedBytes[7] = binary.buffer[6];

    const guid = new Guid(rearrangedBytes);
    return guid;
}
