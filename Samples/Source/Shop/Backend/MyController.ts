// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IMongoDatabase } from '@dolittle/vanir-backend';
import { Controller, Get, Route } from '@tsoa/runtime';
import { injectable } from 'tsyringe';

@Route('/api/typescript/my-controller')
@injectable()
export class MyController extends Controller {

    constructor(private readonly _db: IMongoDatabase) {
        super();
    }

    @Get()
    async doStuff(): Promise<void> {
    }
}
