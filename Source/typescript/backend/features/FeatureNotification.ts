// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Field, ObjectType } from 'type-graphql';
import { FeatureDefinition } from './FeatureDefinition';

@ObjectType()
export class FeatureNotification {
    @Field(type => [FeatureDefinition])
    features: FeatureDefinition[] = [];
}
