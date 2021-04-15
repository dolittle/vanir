// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Guid } from '@dolittle/rudiments';
import { Constructor } from '@dolittle/types';
import { buildSchema, Field, FieldResolver, MiddlewareFn, ObjectType, Query, Resolver, ResolverData, Root } from 'type-graphql';
import { GraphQLSchema } from 'graphql';

import { GuidScalar } from '.';
import { container } from 'tsyringe';
import { BrokenRuleErrorInterceptor } from './BrokenRuleErrorInterceptor';

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


export async function getSchemaFor(resolvers: Constructor[]): Promise<GraphQLSchema> {

    const actualResolvers = resolvers.length > 0 ? resolvers as any : [NoQueries];


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
        ]
    });

    // Filter out all resolvers that are decorated with graphRoot() - this means they shouldn't be on root
    // Wrap these in GraphQLObjectType... :

    /*
    fields: () => ({
            users: { type: UserQueries, resolve: () => [] },
        }) as any,

    export const UserQueries = new GraphQLObjectType<any, any, any>({
        name: 'UserQueries',
        fields: () => ({
            getByEmail: {
                type: UserType,
                description: UserType.description,
                args: typedArgs<GetByEmailRequest>({
                    locale: { type: GraphQLNonNull(GraphQLString) },
                    email: { type: GraphQLNonNull(GraphQLString) },
                }),
                resolve: (_: any, args: GetByEmailRequest) => userService.getByEmail(args.email),
            },
        }),
    });
    */

    const config = schema.toConfig();
    const queryType = config.query?.toConfig();
    schema = new GraphQLSchema(config);


    return schema;
}
