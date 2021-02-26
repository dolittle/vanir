# Anatomy of a backend

This document describes the anatomy of a backend.

## Package.json

Within the `Backend` folder there should be a [`package.json`](./package-json.md) file that describes the backend.

### Scripts

Within the `package.json` it is good practice to have the shorthand scripts for being able to run things.
It is also preferable to provide scripts that provide a watch type of workflow. Part of the scripts could be also to provide
scripts for runnings tests (specs - BDD) and to perform linting on the code.
In addition, there should be a way for the Continuous Integration pipelines to build the code.

For a backend, this could typically look like below:

```json
{
    "scripts": {
        "dev": "nodemon --inspect=0 -e ts --exec node -r ts-node/register index.ts",
        "start": "ts-node index.ts",
        "clean": "tsc -b --clean",
        "build": "yarn clean && webpack --mode=production",
        "test": "mocha",
        "lint": "eslint '**/*.{js,ts,tsx}' --quiet --fix",
        "lint:ci": "eslint '**/*.{js,ts,tsx}' --quiet",
        "ci": "yarn clean && yarn lint:ci && tsc -b && yarn test"
    }
}
```

> Notice there are 2 different lint scripts. One for the CI and one for local. The point of this is that the local one
> is set up to perform any fixes. We don't want the CI pipeline to perform these and hide any issues, since it doesn't
> commit it back.

### Nodemon

As you can see from the scripts above, its using something called [nodemon](https://nodemon.io).
The purpose of this tool is to provide a watch type of workflow, whenever code changes - it automatically restarts and
performs any tasks you want it to perform before it all starts up again.

The configuration itself can sit directly on the `package.json` file. Below is a sample of this that
configures it to watch a couple of folders for changes, ignore the `dist` folder and any declaration files.
In the scripts section, notice the usage of it with `nodemon ./index.ts`.

```json
{
    "nodemonConfig": {
        "restartable": "rs",
        "ignore": [
            "dist",
            "*.d.ts"
        ],
        "execMap": {
            "ts": "ts-node"
        },
        "watch": [
            "./",
            "../../Shared/Backend",
            "../../Shared/DependencyInversion"
        ],
        "ext": "ts"
    }
}
```

## Entrypoint

In the backend you would then have a starting point for the microservice; `index.ts`.
Leveraging the shared backend setup.

```javascript
import 'reflect-metadata';
import { Host } from '@dolittle/vanir-backend';

(async () => {
    await Host.start({
        graphQLResolvers: []            // Array of graphql resolvers
        expressCallback: (app) => {
        },
        dolittleCallback: (dolittle) => {
        }
    });
})();
```

Any GraphQL resolvers can then be added to the array of graphql resolvers, be it queries or mutations.

> Apollo crashes if you add mutations here and no query - if all you have are mutations, you should add a query that doesn't do anything.

## APIs

I you're providing an API from your backend, the shared infrastructure comes with support for something called [TSOA](https://tsoa-community.github.io/docs/introduction.html#goal).
This enables you to easily create REST based API controllers that automatically generate [swagger](https://swagger.io) documentation.
If you want to leverage this, all you need to do is add a file called `tsoa.json` to your project and configure it like this:

```json
{
    "entryFile": "index.ts",
    "noImplicitAdditionalProperties": "throw-on-extras",
    "controllerPathGlobs": [
        "**/*Controller.ts"
    ],
    "spec": {
      "outputDirectory": "./",
      "specVersion": 3
    },
    "routes": {
        "routesDir": "./",
        "iocModule": "../../Shared/Backend/tsoa/ioc"
    }
}
```

Then update the `dev` script in the `package.json` file to be something like this:

```json
{
    "scripts": {
        "dev": "cross-env NODE_TLS_REJECT_UNAUTHORIZED=0 concurrently \"nodemon -x tsoa spec-and-routes\" \"nodemon index.ts\"",
    }
}
```

This will by convention pick up any files suffixed with **Controller** and generate a file called `routes.ts` and a `swagger.json` file.

In the `index.ts` file we'll need to add a couple of things.
Start by adding the following import statements:

```typescript
import { RegisterRoutes } from './routes';
import * as swaggerDoc from './swagger.json';
```

Within the `Host.start()` call block, you can now add the swagger doc into it:

```javascript
import 'reflect-metadata';
import { Host } from '@dolittle/vanir-backend';
import { RegisterRoutes } from './routes';
import * as swaggerDoc from './swagger.json';

(async () => {
    await Host.start({
        swaggerDoc,                     // This is the swagger doc.
        expressCallback: (app) => {
            RegisterRoutes(app);        // Register the routes
        }
    });
})();
```

With this, you'll now have a new swagger endpoint and all your APIs accessible, prefixed with what you have set as prefix.
You should therefor be able to navigate to the URL e.g. http://localhost:3003/_/mymicroservice/api/swagger.

Read more about TSOA and concrete samples [here](https://tsoa-community.github.io/docs/examples.html).
