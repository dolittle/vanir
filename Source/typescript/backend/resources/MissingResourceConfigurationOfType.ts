// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TenantId } from '@dolittle/sdk.execution';
import { Constructor } from '@dolittle/types';

/**
 * Exception that is thrown when the specific resource configuration type is missing for a specific tenant.
 */
export class MissingResourceConfigurationOfType extends Error {
    constructor(tenantId: TenantId, type: Constructor) {
        super(`Missing resource configuration of type '${type.name}' for tenant '${tenantId}'`);
    }
}
