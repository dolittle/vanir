import { Guid } from '@dolittle/rudiments';
import { InputType, Field } from 'type-graphql';
import { MinLength } from 'class-validator';

@InputType({ description: 'This is used for creating an application' })
export class CreateApplication {

    @Field()
    applicationId!: Guid;

    @Field()
    @MinLength(1)
    name!: string;
};
