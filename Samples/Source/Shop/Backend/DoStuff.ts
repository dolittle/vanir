import { Field, InputType } from 'type-graphql';
import { Guid } from '@dolittle/rudiments';
import {Length } from 'class-validator';

@InputType()
export class DoStuff {

    @Field()
    id!: Guid;

    @Field()
    something!: number;

    @Field()
    @Length(5, 10)
    someString!: string;
}
