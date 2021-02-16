# Getting started

Once you have a React frontend project up and running, you need is to add the Vanir React package:

```shell
$ yarn add @dolittle/vanir-react
```

In your starting point, typically `index.tsx` you need a to do a couple of things.
At the top of your file you'll need to import a package called `reflect-metadata`:

```tsx
import 'reflect-metadata';
```

This package enables reflection metadata that is leveraged by the [Tsyringe IoC container](https://github.com/Microsoft/tsyringe)
and can also be useful for other things.

The next thing you'll need is to hook it all up:

```tsx
import 'reflect-metadata';

import React from 'react';
import ReactDOM from 'react-dom';

import { Bootstrapper } from '@dolittle/vanir-react';
import { VersionInfo } from '@dolittle/vanir-web';

const version: VersionInfo = {
    version: '1.0.0',
    built: '',
    commit: ''
};

ReactDOM.render(
    <Bootstrapper name="MyMicroservice" prefix="" version={version}>
        <div>Hello world</div>
    </Bootstrapper>,
    document.getElementById('root')
);
```

The `Bootstrapper` component configures the IoC and sets up all the services needed, such as the GraphQL datasource for easily
connecting to the backend. In the props you'll see the `prefix` - this is the prefix that your application is expected to be
at and the GraphQL endpoint will be relative to. So for instance you could have your microservice be located at `/my-microservice`
and then the GraphQL endpoint would be `/my-microservice/graphql`. The prefix then should be `/my-microservice`.
This is detailed more in [composition](../composition.md).
