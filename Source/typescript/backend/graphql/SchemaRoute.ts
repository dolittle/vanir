// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GraphQLField, GraphQLFieldConfig, GraphQLFieldConfigMap, argsToArgsConfig } from 'graphql/type/definition';

export class SchemaRoute {
    readonly children: SchemaRoute[] = [];
    readonly items: GraphQLFieldConfig<any, any>[] = [];

    constructor(readonly path: string, readonly localName: string, readonly typeName: string) { }

    addChild(child: SchemaRoute) {
        this.children.push(child);
    }

    addItem(item: GraphQLField<any, any>) {
        this.items.push({
            description: item.description,
            type: item.type,
            args: argsToArgsConfig(item.args),
            resolve: item.resolve,
            subscribe: item.subscribe,
            deprecationReason: item.deprecationReason,
            extensions: item.extensions
        });
    }
}
