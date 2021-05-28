# Feature Toggling

Vanir provides an end to end holistic approach to feature toggling.

## File

At the core of the system sits a file that is expected to be located at `.dolittle/features.json` within the
running backend. The expected format is as follows:

```json
{
    <feature-name>: {
        "description": <human friendly description of the feature>,
        "toggles": [
            {
                "type": "Boolean",   // Boolean is currently the only supported type
                "isOn": true/false   // Specific data for the Boolean toggle type
            }
        ]

    }
}
```

## Specific implementations and usages

Below you'll find more details about specific implementations that is built on top of the file.

| Type | Description |
| ---- | ----------- |
| [GraphQL](./DotNET/index.md) | Details on expected GraphQL schemas |
| [C#](./DotNET/index.md) | Details on how to leverage it in C# |
| [TypeScript](./typescript/index.md) | Details on how to leverage it in TypeScript |
| [CLI](./cli/index.md) | Details on how to work with features from the Vanir CLI |
| [React](./react/index.md) | Details on how to leverage it from React in the frontend |
