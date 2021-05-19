import { Field, ObjectType } from 'type-graphql';
import { IFeatureToggleDefinition } from '@dolittle/vanir-features';


@ObjectType()
export class FeatureToggleDefinition implements IFeatureToggleDefinition {
    @Field()
    type!: string;

    @Field()
    isOn!: boolean;
}
