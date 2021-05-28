import { ObjectType, Field } from 'type-graphql';
import { Guid } from '@dolittle/rudiments';



@ObjectType()
export class Nothing {
    @Field({ name: 'id' })
    _id?: Guid;
}
