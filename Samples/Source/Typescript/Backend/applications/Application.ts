import { Guid } from '@dolittle/rudiments';
import { guid } from '@dolittle/vanir-backend';
import { ObjectType, Field } from 'type-graphql';

@ObjectType()
export class Application {
    @Field()
    @guid()
    _id!: Guid;

    @Field()
    name!: string;
}
