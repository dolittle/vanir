// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GraphQLField, GraphQLFieldConfig, GraphQLFieldConfigMap, argsToArgsConfig } from 'graphql/type/definition';
import { GraphQLObjectType } from 'graphql';
import { PubSub } from 'graphql-subscriptions';

const pubSub = new PubSub();

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

    get isRoot() {
        return this.path === '';
    }

    toGraphQLObjectType(): GraphQLObjectType {
        const fields: GraphQLFieldConfigMap<any, any> = {};
        for (const route of this.children) {
            const type = route.toGraphQLObjectType();
            fields[route.localName] = {
                type,
                resolve: () => []
            };
        }

        for (const item of this.items) {
            fields[item.name] = {
                description: item.description,
                type: item.type,
                args: argsToArgsConfig(item.args),
                resolve: item.resolve,
                subscribe: item.subscribe,
                deprecationReason: item.deprecationReason,
                extensions: item.extensions
            };
        }

        const type = new GraphQLObjectType({
            name: this.typeName,
            fields
        });

        return type;
    }
}
