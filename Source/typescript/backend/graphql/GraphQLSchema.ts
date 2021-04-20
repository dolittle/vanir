// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Guid } from '@dolittle/rudiments';
import { Constructor } from '@dolittle/types';
import { buildSchema, Field, FieldResolver, MiddlewareFn, ObjectType, Query, Resolver, ResolverData, Root } from 'type-graphql';
import { GraphQLSchema, GraphQLObjectType, GraphQLFieldConfig, GraphQLArgumentConfig, GraphQLFieldConfigMap } from 'graphql';

import { GuidScalar } from '.';
import { container } from 'tsyringe';
import { BrokenRuleErrorInterceptor } from './BrokenRuleErrorInterceptor';
import { any } from 'nconf';
import { GraphQLSchemaRouteBuilder } from './GraphQLSchemaRouteBuilder';

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


export interface GraphQLFieldConfig<
  TSource,
  TContext,
  TArgs = { [argName: string]: any }
> {
  description?: Maybe<string>;
  type: GraphQLOutputType;
  args?: GraphQLFieldConfigArgumentMap;
  resolve?: GraphQLFieldResolver<TSource, TContext, TArgs>;
  subscribe?: GraphQLFieldResolver<TSource, TContext, TArgs>;
  deprecationReason?: Maybe<string>;
  extensions?: Maybe<
    Readonly<GraphQLFieldExtensions<TSource, TContext, TArgs>>
  >;
  astNode?: Maybe<FieldDefinitionNode>;
}

export interface GraphQLArgumentConfig {
  description?: Maybe<string>;
  type: GraphQLInputType;
  defaultValue?: any;
  deprecationReason?: Maybe<string>;
  extensions?: Maybe<Readonly<GraphQLArgumentExtensions>>;
  astNode?: Maybe<InputValueDefinitionNode>;
}
    */


    const config = schema.toConfig();
    //const queryType = config.query?.toConfig();

    /*
    const namespaceTypes: GraphQLObjectType<any, any>[] = [];

    if (config.query) {
        const rootFields = config.query.getFields();
        const currentFields = {} as GraphQLFieldConfigMap<any, any>;
        for (const fieldName in rootFields) {
            const field = rootFields[fieldName];
            if (field.extensions?.graphRoot) {
                const namespaces = field.extensions?.graphRoot.split('/');
                for (const ns of namespaces) {
                }
            } else {
                currentFields[fieldName] = {
                    description: field.description,
                    type: field.type,
                    resolve: field.resolve,
                    subscribe: field.subscribe,
                    deprecationReason: field.deprecationReason,
                    extensions: field.extensions
                };
            }
        }
    }
    */

    //const allApplications = rootFields!.allApplications;

    /*
    const namespaceType = new GraphQLObjectType<any, any>({
        name: '_namespaceType',
        fields: () => ({
            allApplications: {
                description: allApplications.description,
                type: allApplications.type,
                // args: allApplications.args.map(_ => {
                //     return {
                //         description: _.description,
                //         type: _.type,
                //         defaultValue: _.defaultValue,
                //         deprecationReason: _.deprecationReason,
                //         extensions: _.extensions
                //     } as GraphQLArgumentConfig;
                // }),
                resolve: allApplications.resolve,
                subscribe: allApplications.subscribe,
                deprecationReason: allApplications.deprecationReason,
                extensions: allApplications.extensions
            }
        })
    });
    */

    /*
    const queryType = new GraphQLObjectType<any, any>({
        name: 'Query',
        fields: () => ({
            something: {
                type: namespaceType,
                description: 'This is the namespace',
                resolve: () => []
            }
        })
    });
    */

    GraphQLSchemaRouteBuilder.handleQueries(config);
    GraphQLSchemaRouteBuilder.handleMutations(config);

    //const types = config.types.filter(_ => _.name !== 'Query');
    //config.query = queryType;
    //config.types = [...[queryType, namespaceType], ...types];
    schema = new GraphQLSchema(config);

    return schema;
}
