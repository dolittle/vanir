// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GraphQLField } from 'graphql';

export class SchemaRoute {
    readonly children: SchemaRoute[] = [];
    readonly items: GraphQLField<any, any>[] = [];

    constructor(readonly path: string, readonly localName: string, readonly typeName: string) { }

    addChild(child: SchemaRoute) {
        this.children.push(child);
    }

    addItem(item: GraphQLField<any, any>) {
        this.items.push(item);
    }
}
