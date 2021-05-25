// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Field, ObjectType } from 'type-graphql';
import { IFeatureToggleDefinition } from '@dolittle/vanir-features';


@ObjectType()
export class FeatureToggleDefinition implements IFeatureToggleDefinition {
    @Field()
    type!: string;

    @Field()
    isOn!: boolean;
}
