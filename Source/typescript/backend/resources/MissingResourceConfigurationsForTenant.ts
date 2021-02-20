// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { TenantId } from '@dolittle/sdk.execution';

/**
 * Exception that is thrown when there are no resource configurations for a specific tenant.
 */
export class MissingResourceConfigurationsForTenant extends Error {
    constructor(tenantId: TenantId) {
        super(`Missing resource configurations for '${tenantId}'`);
    }
}
