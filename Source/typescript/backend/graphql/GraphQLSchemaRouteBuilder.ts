// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ResolverAtPath } from './ResolverAtPath';
import { GraphQLField, GraphQLSchemaConfig } from 'graphql';

export class GraphQLSchemaRouteBuilder {
    static handleQueries(config: GraphQLSchemaConfig): void {
        if (config.query) {
            const queries = Object.values(config.query.getFields());
            const resolversAtPath = this.getResolversWithPath(queries);

            const resolversByPath = resolversAtPath.reduce((groups, item) => {
                groups[item.path] = groups[item.path] || [];
                groups[item.path].push(item);
                return groups;
            }, {});

            let i = 0;
            i++;
        }
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
}
