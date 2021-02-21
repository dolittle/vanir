// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import cls from 'cls-hooked';
import { Constructor } from '@dolittle/types';
import { Request, Response, NextFunction } from 'express';

const ns = cls.createNamespace('c5da357c-42b4-4c04-8ff5-2ba3804ede42');

export function MongoDbContextMiddleware(req: Request, res: Response, next: NextFunction) {
    ns.run(() => {
        next();
    });
}


export function setCollectionType(collectionName: string, type: Constructor) {
    ns.set(collectionName, type);
}

export function getCollectionType(collectionName): Constructor | undefined {
    return ns.get(collectionName);
}
