// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ResourceConfiguration } from './ResourceConfiguration';

/**
 * Represents the configuration for a MongoDb connection.
 */
export class MongoDbReadModelsConfiguration extends ResourceConfiguration {
    host!: string;
    database!: string;
    useSSL!: boolean;
}
