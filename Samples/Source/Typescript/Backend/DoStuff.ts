import { Directive, Field, InputType } from 'type-graphql';



@InputType()
export class DoStuff {

    @Field()
    @Directive('@specifiedBy(url: "Use newasdasd")')
    @Directive('@deprecated(reason: "Use newField")')
    something!: string;
}
