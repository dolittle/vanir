// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GraphQLField } from 'graphql';

export class SchemaRouteItem {
    constructor(readonly field:  GraphQLField<any, any>, readonly name: string) { }
}
