// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GraphQLScalarType, GraphQLInt, GraphQLFloat, GraphQLBoolean, GraphQLString } from 'graphql';
import { Kind } from 'graphql/language';
import { GuidScalar } from './GuidScalar';


const scalarsForParsing = [
    GraphQLBoolean,
    GuidScalar,
    GraphQLFloat,
    GraphQLInt,
    GraphQLString
];

export const ObjectScalar = new GraphQLScalarType({
    name: 'Object',
    description: 'Arbitrary object',
    parseValue: (value) => {
        return typeof value === 'object' ? value
            : typeof value === 'string' ? JSON.parse(value)
                : null;
    },
    serialize: (value) => {
        return typeof value === 'object' ? value
            : typeof value === 'string' ? JSON.parse(value)
                : null;
    },
    parseLiteral: (ast) => {
        switch (ast.kind) {
            case Kind.STRING: return JSON.parse(ast.value);
            case Kind.OBJECT: {
                const event: any = {};

                for (const field of ast.fields) {
                    let value: any;

                    for (const scalar of scalarsForParsing) {
                        try {
                            value = scalar.parseLiteral(field.value, {});
                            // eslint-disable-next-line no-empty
                        } catch (ex) { }

                        if (value) {
                            break;
                        }
                    }
                    event[field.name.value] = value;
                }

                return event;

            };
            default: return null;
        }
    }
});

