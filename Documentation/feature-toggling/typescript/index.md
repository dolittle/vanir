# Feature Toggling - TypeScript

The feature system resembles how you would typically use authorization, leveraging an attribute called `[Feature]`.

## GraphQL

The `@feature` decorator is hooked up for GraphQL.

Lets imaging you have the following resolver with a mutation on it:

```typescript
import { Resolver, Mutation } from 'type-graphql';

@Resolver()
export class Kitchen : GraphController
{
    @Mutation(returns => Boolean)
    async bool prepareDish(dish: string, chef: string)
    {
        // Prepare the dish

        return true;
    }
}
```

To be able to switch on or off a feature, you can adorn either the class or the method with the `[Feature]` attribute.

```typescript
import { Resolver, Mutation } from 'type-graphql';
import { feature } from '@dolittle/vanir-features';

@Resolver()
@feature("kitchen")
export class Kitchen : GraphController
{
    @Mutation(returns => Boolean)
    @feature("kitchen.prepare-dish")
    async bool prepareDish(dish: string, chef: string)
    {
        // Prepare the dish

        return true;
    }
}
```

You can now control the specific `kitchen.prepare-dish` feature on the method level, or control all features within the class
with the feature name `kitchen`.
