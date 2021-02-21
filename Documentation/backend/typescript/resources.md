# Resources

All applications has a dependency on resources it uses, typically a database or similar.
These resources are managed for you in the [Dolittle platform](https://dolittle.io/docs/platform/requirements/#1-your-application-must-use-the-resource-system).
And its therefor important that solutions support this. Vanir brings a resource system
out of the box to enable one to get to resource configurations on demand depending on
the context.

## Resources.json

At startup Vanir will look for a file called `resources.json` inside the `.dolittle` folder
within the startup folder of your microservice.

This file should look like the following:

```json
{
    "<tenant identifier>": {
        "readModels": {
            "host": "mongodb://localhost:27017",
            "database": "read_models_<name of microservice>",
            "useSSL": false
        },
        "eventStore": {
            "servers": [
                "localhost"
            ],
            "database": "event_store_<name of microservice>"
        }
    }
}
```

Every tenant should have configurations for each supported resource the system uses.

The `readModels` section is the thing that is used for any [MongoDB](./mongodb.md) database needs.
For the `eventStore` - this is only needed temporarily as the projection engine will eventually live
inside the Runtime.

It is important to remember that this file is a managed file within the Dolittle Platform and you'd
typically only have what is needed to be able to run locally on a dev machine inside this file.

> There is a well known tenant identifier that you can default to for your local development environment.
> We call this the development tenant and has the identifier `445f8ea8-1a6f-40d7-b2fc-796dba92dc44`.
> On the type called `TenantId`in the `@dolittle/sdk.execution` package you'll also see a property called
> `development` that has this same value.

## Resource Configurations

All resources should have a configuration per tenant in the resources file. They are accessible through
a system called `IResourceConfigurations`. It is configured in the [IoC](./ioc.md) and available to be used
in your solution:

```typescript
import { IResourceConfigurations } from '@dolittle/vanir-backend/dist/resources';
import { MongoDbReadModelsConfiguration } from '@dolittle/vanir-backend/dist/mongodb';
import { TenantId } from '@dolittle/sdk.execution';
import { injectable } from 'tsyringe';

@injectable()
export class MyClass {
    constructor(resourceConfigurations: IResourceConfigurations) {
        const configuration = resourceConfiguration.getFor(MongoDbReadModelsConfiguration, TenantId.development);

        // You can now use: configuration.host, configuration.database
    }
}
```
