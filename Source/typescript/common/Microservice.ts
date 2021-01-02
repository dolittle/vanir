// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export type Microservice = {
    id: string;
    name: string;
    version: string;
    commit: string;
    built: string;
    web: boolean
};
