// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ResolverAtPath } from './ResolverAtPath';
import { GraphQLField, GraphQLSchemaConfig, GraphQLObjectType } from 'graphql';
import { SchemaRoute } from './SchemaRoute';

export class GraphQLSchemaRouteBuilder {
    static handleQueries(config: GraphQLSchemaConfig): void {
        if (config.query) {
            config.query = this.buildSchemaRoutesWithItems(config, 'Query', config.query, 'Queries');
        }
    }

    static handleMutations(config: GraphQLSchemaConfig): void {
        if (config.mutation) {
            config.mutation = this.buildSchemaRoutesWithItems(config, 'Mutation', config.mutation, 'Mutations');
        }
    }

    private static buildSchemaRoutesWithItems(config: GraphQLSchemaConfig, rootName: string, rootType: GraphQLObjectType<any, any>, postFix: string): GraphQLObjectType {
        const root = new SchemaRoute('', rootName, rootName);
        const resolvers = Object.values(rootType.getFields());
        const resolversAtPath = this.getResolversWithPath(resolvers);
        this.buildRouteHierarchy(root, resolversAtPath, postFix);

        const newTypes: GraphQLObjectType[] = [];
        const newRoot = root.toGraphQLObjectType(newTypes);
        return newRoot;
    }


    private static getResolversWithPath(fields: GraphQLField<any, any>[]): ResolverAtPath[] {
        return fields.map(_ => {
            const graphRoot = _.extensions?.graphRoot || '';
            return new ResolverAtPath(graphRoot, _);
        }).sort((a, b) => {
            if (a.path < b.path) {
                return -1;
            }
            if (a.path > b.path) {
                return 1;
            }
            return 0;
        });
    }

    private static buildRouteHierarchy(root: SchemaRoute, resolversAtPath: ResolverAtPath[], postFix: string) {
        const resolversByPath: { [key: string]: ResolverAtPath[] } = resolversAtPath.reduce((groups, item) => {
            groups[item.path] = groups[item.path] || [];
            groups[item.path].push(item);
            return groups;
        }, {});

        const routesByPath: { [key: string]: SchemaRoute } = {};
        routesByPath[''] = root;

        for (const path in resolversByPath) {

            let current = '';
            const segments = path.split('/');
            let currentRoute: SchemaRoute | undefined;
            let parentRoute: SchemaRoute = root;

            for (const segment of segments) {
                current = `${current}${(current.length > 0 ? '/' : '')}${segment}`;
                if (routesByPath.hasOwnProperty(current)) {
                    currentRoute = routesByPath[current];
                } else {
                    currentRoute = new SchemaRoute(current, segment, `_${segment}${postFix}`);
                    routesByPath[current] = currentRoute;
                    parentRoute?.addChild(currentRoute);
                }

                parentRoute = currentRoute!;
            }

            for (const resolver of resolversByPath[path]) {
                currentRoute?.addItem(resolver.resolver);
            }
        }
    }
}
