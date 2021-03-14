# Host

The starting point for a Vanir application is to call `start` on the `Host` object.
In your code you would have a starting point for your microservice; `index.ts`.

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

> Note:
> Apollo crashes if you add mutations here and no query - if all you have are mutations, you should add a query that doesn't do anything.
> This is a known issue ([#137](https://github.com/dolittle-entropy/vanir/issues/137))

## APIs

If you're providing an API from your backend, the shared infrastructure comes with support for something called [TSOA](https://tsoa-community.github.io/docs/introduction.html#goal).
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
