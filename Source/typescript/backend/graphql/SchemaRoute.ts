// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { SchemaRouteItem } from './SchemaRouteItem';

export class SchemaRoute {
    readonly children: SchemaRoute[] = [];
    readonly items: SchemaRouteItem[] = [];

    constructor(readonly path: string, readonly localName: string, readonly typeName: string) { }

    addChild(child: SchemaRoute) {
        this.children.push(child);
    }

    addItem(item: SchemaRouteItem) {
        this.items.push(item);
    }
}
