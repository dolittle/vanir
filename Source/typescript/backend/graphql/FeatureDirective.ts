// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { FeatureDecorators } from '@dolittle/vanir-features';
import { GraphQLSchema, GraphQLField, GraphQLDirective, DirectiveLocation, GraphQLString } from 'graphql';
import { SchemaDirectiveVisitor } from 'graphql-tools';
import { getMetadataStorage } from 'type-graphql/dist/metadata';
import { Constructor } from '@dolittle/types';
import { container } from 'tsyringe';
import { IFeatureToggles } from '@dolittle/vanir-features';
import { constructor } from '@dolittle/vanir-dependency-inversion';
import { FeatureDisabled } from './FeatureDisabled';

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
        const resolve = field.resolve;
        const feature = this.args.name;

        field.resolve = async function (source, { append, ...otherArgs }, context, info) {
            const featureToggles = container.resolve(IFeatureToggles as constructor<IFeatureToggles>);
            if (!featureToggles.isOn(feature)) {
                throw new FeatureDisabled(feature);
            }
            const result = await resolve!.call(this, source, otherArgs, context, info);
            return result;
        };
    }
}

