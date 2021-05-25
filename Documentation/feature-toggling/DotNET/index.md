# Feature Toggling - C#

The feature system resembles how you would typically use authorization, leveraging an attribute called `[Feature]`.

## GraphQL

The `[Feature]` attribute is hooked up for GraphQL when using the `GraphController` system.

Lets imaging you have the following controller with a mutation on it:

```csharp
using Dolittle.Vanir.Backend.GraphQL;

public class Kitchen : GraphController
{
    [Mutation]
    public bool PrepareDish(string dish, string chef)
    {
        // Prepare the dish

        return true;
    }
}
```

To be able to switch on or off a feature, you can adorn either the class or the method with the `[Feature]` attribute.

```csharp
using Dolittle.Vanir.Backend.GraphQL;
using Dolittle.Vanir.Backend.Features;

[Feature("kitchen")]
public class Kitchen : GraphController
{
    [Mutation]
    [Feature("kitchen.prepare-dish")]
    public bool PrepareDish(string dish, string chef)
    {
        // Prepare the dish

        return true;
    }
}
```

You can now control the specific `kitchen.prepare-dish` feature on the method level, or control all features within the class
with the feature name `kitchen`.
