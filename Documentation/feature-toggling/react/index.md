# Feature Toggling - React

In React you have a **hook** that enables you to simply get a reference to a feature.
The hook leverages the underlying [GraphQL subscription](../graphql/index.md) to know when it
changes and automatically triggers a React render if it changes state.

```tsx
import React from 'react';
import { useFeature } from '@dolittle/vanir-react';

export const Kitchen = () => {
    const prepareDishEnabled = useFeature('kitchen.prepare-dish');

    return {
        <>
            Prepare dish enabled: {{prepareDishEnabled}}
        </>
    };
};
```
