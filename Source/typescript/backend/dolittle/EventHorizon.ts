// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export type EventHorizon = {
    microservice: string;
    tenant: string;
    stream: string;
    partition: string;
    scope: string;
};
