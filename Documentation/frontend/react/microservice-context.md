# Microservice Context

When using the `<Bootstrapper/>` as described in the [getting started guide](./getting-started.md)
a context is established at the top level that can be leveraged throughout the solution.

The `<MicroserviceContext/>` is a [React Context](https://reactjs.org/docs/context.html) object that
can be consumed anywhere:

```tsx
import React from 'react';
import { MicroserviceContext } from '@dolittle/vanir-react';

export const MyComponent = () = {
    return (
        <MicroserviceContext.Consumer>
            {value => {
                /*
                    Value is of type MicroserviceConfiguration and holds the information set
                    by the bootstrapper:

                    - name
                    - prefix
                    - version
                */
            }}
        </MicroserviceContext.Consumer>
    );
};
```
