// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GraphQLScalarType } from 'graphql';
import { Kind } from 'graphql/language';

export const ObjectScalar = new GraphQLScalarType({
    name: 'Object',
    description: 'Arbitrary object',
    parseValue: (value) => {
        return typeof value === 'object' ? value
            : typeof value === 'string' ? JSON.parse(value)
                : null
    },
    serialize: (value) => {
        return typeof value === 'object' ? value
            : typeof value === 'string' ? JSON.parse(value)
                : null
    },
    parseLiteral: (ast) => {
        switch (ast.kind) {
            case Kind.STRING: return JSON.parse(ast.value)
            case Kind.OBJECT: throw new Error(`Not sure what to do with OBJECT for ObjectScalarType`)
            default: return null
        }
    }
});

