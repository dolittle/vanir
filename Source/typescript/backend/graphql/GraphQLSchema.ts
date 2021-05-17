// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Guid } from '@dolittle/rudiments';
import { buildSchema, Field, ObjectType, Query, Resolver, ResolverData } from 'type-graphql';
import { GraphQLSchema } from 'graphql';

import { GuidScalar } from './GuidScalar';
import { container } from 'tsyringe';
import { BrokenRuleErrorInterceptor } from './BrokenRuleErrorInterceptor';
import { GraphQLSchemaRouteBuilder } from './GraphQLSchemaRouteBuilder';
import { BackendArguments } from '../BackendArguments';
import { Configuration } from '../Configuration';
import { SchemaDirectiveVisitor } from 'graphql-tools';
import { FeatureDirective } from './FeatureDirective';

@ObjectType()
class Nothing {
    @Field({ name: 'id' })
    _id?: Guid;
}

@Resolver(Nothing)
class NoQueries {
    @Query(returns => [Nothing])
    async noresults() {
        return [];
    }
}

export async function getSchemaFor(configuration: Configuration, backendArguments: BackendArguments): Promise<GraphQLSchema> {
    const resolvers = backendArguments.graphQLResolvers || [];
    const actualResolvers = resolvers.length > 0 ? resolvers as any : [NoQueries];

    FeatureDirective.extendTypesAndFields();

    let schema = await buildSchema({
        resolvers: actualResolvers,
        globalMiddlewares: [BrokenRuleErrorInterceptor],
        container: {
            get(someClass: any, resolverData: ResolverData<any>): any | Promise<any> {
                return container.resolve(someClass);
            }
        },
        scalarsMap: [
            { type: Guid, scalar: GuidScalar }
        ],
        directives: [

        ]
    });

    SchemaDirectiveVisitor.visitSchemaDirectives(schema, {
        feature: FeatureDirective
    });

    const config = schema.toConfig();

    GraphQLSchemaRouteBuilder.handleQueries(config);
    GraphQLSchemaRouteBuilder.handleMutations(configuration, config, backendArguments);

    config.types = [];
    schema = new GraphQLSchema(config);

    return schema;
}




