// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Guid } from '@dolittle/rudiments';
import { Constructor } from '@dolittle/types';
import { buildSchema, Field, ObjectType, Query, Resolver, ResolverData } from 'type-graphql';
import { GraphQLNamedType, print, isSpecifiedScalarType, isSpecifiedDirective, GraphQLArgument, GraphQLEnumType, GraphQLEnumValue, GraphQLField, GraphQLInputField, GraphQLInputObjectType, GraphQLInterfaceType, GraphQLObjectType, GraphQLScalarType, GraphQLSchema, GraphQLUnionType, InputValueDefinitionNode } from 'graphql';

import { GuidScalar } from '.';
import { container } from 'tsyringe';

import { visitSchema, SchemaVisitor } from 'graphql-tools/dist/schemaVisitor';

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

class MyVisitor implements SchemaVisitor {
    schema!: GraphQLSchema;
    visitSchema(schema: GraphQLSchema): void {

    }
    visitScalar(scalar: GraphQLScalarType): void | GraphQLScalarType | null {

    }
    visitObject(object: GraphQLObjectType<any, any>): void | GraphQLObjectType<any, any> | null {

    }
    visitFieldDefinition(field: GraphQLField<any, any, { [key: string]: any; }>, details: { objectType: GraphQLInterfaceType | GraphQLObjectType<any, any>; }): void | GraphQLField<any, any, { [key: string]: any; }> | null {

    }
    visitArgumentDefinition(argument: GraphQLArgument, details: { field: GraphQLField<any, any, { [key: string]: any; }>; objectType: GraphQLInterfaceType | GraphQLObjectType<any, any>; }): void | GraphQLArgument | null {

    }
    visitInterface(iface: GraphQLInterfaceType): void | GraphQLInterfaceType | null {

    }
    visitUnion(union: GraphQLUnionType): void | GraphQLUnionType | null {

    }
    visitEnum(type: GraphQLEnumType): void | GraphQLEnumType | null {

    }
    visitEnumValue(value: GraphQLEnumValue, details: { enumType: GraphQLEnumType; }): void | GraphQLEnumValue | null {

    }
    visitInputObject(object: GraphQLInputObjectType): void | GraphQLInputObjectType | null {

    }
    visitInputFieldDefinition(field: GraphQLInputField, details: { objectType: GraphQLInputObjectType; }): void | GraphQLInputField | null {
        let newField: GraphQLInputField = {
            ...field, ... {
                description: 'Awesomeness',
                extensions: {
                    'hello': 'world'
                }
            }
        };

        /*
        let astNode: InputValueDefinitionNode = {
            ...field.astNode, ... {
                directives: [{
                    name: {
                        value: 'deprecated'
                    },
                    arguments: [
                        {
                            name: 'reason',
                            value: 'Because I can'
                        }


                    ]


                }]
            }
        } as any;

        newField.astNode = astNode;*/

        return newField;
    }

}

// https://github.com/graphql/graphql-js/issues/869
export function printSchemaWithDirectives(schema: GraphQLSchema) {
    const str = Object
        .keys(schema.getTypeMap())
        .filter(k => !k.match(/^__/))
        .reduce((accum, name) => {
            const type = schema.getType(name)! as GraphQLNamedType;
            const val = !isSpecifiedScalarType(type)
                ? accum += `${print(type.astNode!)}\n`
                : accum;

            return val;
        }, '');

    return schema
        .getDirectives()
        .reduce((accum, d) => {
            const val = !isSpecifiedDirective(d)
                ? accum += `${print(d.astNode!)}\n`
                : accum;
            return val;
        }, str + `${print(schema.astNode!)}\n`);
}

export async function getSchemaFor(resolvers: Constructor[]): Promise<GraphQLSchema> {

    const schema = await buildSchema({
        resolvers: resolvers.length > 0 ? resolvers as any : [NoQueries],
        emitSchemaFile: {

        },
        container: {
            get(someClass: any, resolverData: ResolverData<any>): any | Promise<any> {
                return container.resolve(someClass);
            }
        },
        scalarsMap: [
            { type: Guid, scalar: GuidScalar }
        ]

    });

    visitSchema(schema, (type, methodName) => {
        console.log(methodName);
        if (methodName === 'visitInputFieldDefinition') {
            return [new MyVisitor()]
        }
        return [];
    });

    const schemaAsString = printSchemaWithDirectives(schema);

    return schema;
}
