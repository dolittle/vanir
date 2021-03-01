// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import cls from 'cls-hooked';
import { Constructor } from '@dolittle/types';
import { HostContext } from '../HostContext';

const CollectionPrefix = 'MongoDbCollection:';

export function setCollectionType(collectionName: string, type: Constructor) {
    HostContext.set(`${CollectionPrefix}${collectionName}`, type);
}

export function getCollectionType(collectionName): Constructor | undefined {
    return HostContext.get(`${CollectionPrefix}${collectionName}`);
}
