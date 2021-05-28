// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import fs from 'fs';
import { EventHorizon } from './EventHorizon';
import path from 'path';
import { TenantId } from '@dolittle/sdk.execution';

type EventHorizonsDefinition = { [key: string]: EventHorizon[] }

export class EventHorizons extends Map<TenantId, EventHorizon[]> {
    private static readonly _pathToFile = path.join(process.cwd(), '.dolittle', 'event-horizons.json');

    constructor(eventHorizons: EventHorizonsDefinition) {
        super();

        for (const tenant of Object.keys(eventHorizons)) {
            this.set(TenantId.from(tenant), eventHorizons[tenant]);
        }
    }

    static load(): EventHorizons {
        if (fs.existsSync(EventHorizons._pathToFile)) {
            const json = fs.readFileSync(EventHorizons._pathToFile).toString();
            const definition = JSON.parse(json) as EventHorizonsDefinition;
            return new EventHorizons(definition);
        }

        return new EventHorizons({});

    }
}

