// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Portal } from './Portal';

export type Application = {
    id: string;
    name: string;
    tenant: string;
    license: string;
    containerRegistry: string;
    portal: Portal;
    microservices: string[]
};
