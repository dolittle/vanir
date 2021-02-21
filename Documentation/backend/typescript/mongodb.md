# MongoDB

The out of the box experience with MongoDB is focused on bringing the [native mongo db driver](https://www.npmjs.com/package/mongodb) experience
without being too obtrusive. But since applications running on the Dolittle platform has a [resource requirement](https://dolittle.io/docs/platform/requirements/#1-your-application-must-use-the-resource-system);
Vanir has a couple of helpers to make it easier to get the [resource configuration](./resources.md) and
wrappers on top of this for MongoDB specific to make it easy to get the correct MongoDB database instance.

Since Dolittle is designed and built from the ground up with multi-tenancy in mind, the resource configuration can vary
from request to request. On the HTTP header, the Dolittle platform will inject the correct Tenant Id (see [here](../../microservice.md)).
This is then used by custom middlewares to resolve resources correctly.


## MongoDbReadModelsConfiguration

The actual configuration from a `resources.json` file is read and parsed and translated into types that can be leveraged.
ReadModel configurations for MongoDb is defined in a type called `MongoDbReadModelsConfiguration` that looks like the following:

```typescript
{
    host: string;
    database: string;
    useSSL: boolean;
}
```

## Express Middleware

For the serialization subsystem to work properly out of the box, it relies on being aware of the type mapped to a collection
for the web request it is being used in. To achieve this there is an Express Middleware that can be used.
The default setup gets set up with this, but if you have your own setup and want to leverage
this functionality - you can do so through the following:

```typescript
import express from 'express';
import {MongoDbMiddleware } from '@dolittle/vanir-backend/dist/mongodb';

const app = express();
app.use(MongoDbMiddleware);
```

## Working with collections

When your system needs to access a specific collection, it can go through the `IMongoDatabase` interface to do so.
The advantage of doing so is that it will hook up the serialization sub system to work unobtrusively for you as a developer.

That way you don't have to hand-code conversions going back and forth to MongoDB, but rely on serializers already having
been set up to do the job for you.

The API offers a way to get collections by associating a schema/document type. This is the type it leverages to make
the serialization sub system work.

Lets say you have an API controller:

```typescript
import { IMongoDatabase } from '@dolittle/vanir-backend';
import { guid } from '@dolittle/vanir-backend/dist/mongodb';
import { Controller, Get, Route } from '@tsoa/runtime';
import { injectable } from 'tsyringe';
import { Guid } from '@dolittle/rudiments';

class MyType {

    @guid()
    _id!: Guid;

    something: number = 0;
}

@Route('/api/typescript/my-controller')
@injectable()
export class MyController extends Controller {

    constructor(private readonly _db: IMongoDatabase) {
        super();
    }

    @Post()
    async insertThings(): Promise<void> {
        const collection = await this._db.collection(MyType);
        await collection.insertOne({
            _id: Guid.create(),
            something: 42
        });
    }

    @Get()
    async doStuff(): Promise<MyType[]> {
        const result = await collection.find().toArray();
        return result;
    }
}
```

## Custom types

The serialization subsystem introduces the concept of a `CustomType` - these are the ones used to convert to and from
MongoDBs BSON binary.

Creating your own custom type is easy, all you need to do is to implement the abstract class `CustomType`:

```typescript
import { CustomType } from '@dolittle/vanir-backend/dist/mongodb';
import { Binary } from 'mongodb';

export class MyCustomType implements CustomType<MyType> {
    constructor() {
        super(Mytype);
    }

    toBSON(value: MyType): Binary {
        // Convert to Binary
    }

    fromBSON(value: Binary): MyType {
        // Convert from Binary to your type
    }
}
```

This custom type can now be used on your model:

```typescript
export class MyModel {
    @customType(MyCustomType)
    something!: MyType;
}
```

## Guid

For `Guid` there already is a build in custom type serializer, with a custom decorator as well:

```typescript
import { Guid } from '@dolittle/rudiments';
import { guid } from '@dolittle/vanir-backend/dist/mongodb';

export class MyModel {
    @guid()
    _id: Guid;
}
```
