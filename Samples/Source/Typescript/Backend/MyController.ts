// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

    @Get()
    async doStuff(): Promise<MyType[]> {
        const collection = await this._db.collection(MyType);
        await collection.insertOne({
            _id: Guid.create(),
            something: 42
        });
        const result = await collection.find().toArray();
        return result;
    }
}
