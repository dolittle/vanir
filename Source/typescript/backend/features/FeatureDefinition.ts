// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Field, ObjectType } from 'type-graphql';
import { IFeatureDefinition, IFeatureToggleDefinition } from '@dolittle/vanir-features';
import { FeatureToggleDefinition } from './FeatureToggleDefinition';

@ObjectType()
export class FeatureDefinition implements IFeatureDefinition {
    @Field()
    name!: string;

    @Field()
    description!: string;

    @Field(() => [FeatureToggleDefinition])
    toggles!: IFeatureToggleDefinition[];
}
