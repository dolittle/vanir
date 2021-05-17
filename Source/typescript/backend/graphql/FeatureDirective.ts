// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FeatureDecorators } from '@dolittle/vanir-features';
import { GraphQLSchema, GraphQLField, GraphQLDirective, DirectiveLocation, GraphQLString } from 'graphql';
import { SchemaDirectiveVisitor } from 'graphql-tools';
import { getMetadataStorage } from 'type-graphql/dist/metadata';
import { Constructor } from '@dolittle/types';

export class FeatureDirective extends SchemaDirectiveVisitor {

    static extendTypesAndFields() {
        const storage = getMetadataStorage();

        const extendField = (target: Constructor, fieldName: string, features: string[]) => {

            for (let feature of features) {
                storage.collectDirectiveFieldMetadata({
                    target,
                    fieldName,
                    directive: {
                        nameOrDefinition: `@feature(name:"${feature}")`,
                        args: {}
                    }
                });
            }
        }

        for (const [target, features] of FeatureDecorators.methodFeatures) {
            extendField(target.target, target.method, features);
        }

        for (const [target, features] of FeatureDecorators.classFeatures) {
            for (let property of Object.getOwnPropertyNames(target.prototype)) {
                extendField(target, property, features);
            }
        }
    }

    static getDirectiveDeclaration(directiveName: string, schema: GraphQLSchema): GraphQLDirective {
        return new GraphQLDirective({
            name: directiveName,
            args: {
                name: {
                    description: 'Name of the feature',
                    type: GraphQLString
                }
            },
            locations: [
                DirectiveLocation.FIELD
            ]
        });
    }

    visitFieldDefinition(field: GraphQLField<any, any>, details: any) {
        var i = 0;
        i++;
    }
}
