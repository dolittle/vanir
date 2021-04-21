import { graphRoot } from '@dolittle/vanir-backend';
import { Resolver, Query, Arg, ObjectType, Field } from 'type-graphql';

@ObjectType()
export class Danish {
    @Field()
    name: string = 'Danish';
}

@ObjectType()
export class Recipe {
    @Field()
    name: string = 'Stuff';
}

@Resolver()
@graphRoot('groceryStore/bakery')
export class Bakery {
    @Query(() => [Danish], { name: 'search' })
    @graphRoot('pastries')
    async searchPastries(@Arg('nameLike') nameLike: string): Promise<Danish[]> {
        return [
            new Danish()
        ];
    }

    @Query(() => Recipe, { name: 'recipe' })
    @graphRoot('pastries')
    async retrieveRecipe(@Arg('id') id: string): Promise<Recipe> {
        return new Recipe();
    }
}
