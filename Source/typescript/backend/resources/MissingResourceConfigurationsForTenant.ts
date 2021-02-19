import { TenantId } from '@dolittle/sdk.execution';

/**
 * Exception that is thrown when there are no resource configurations for a specific tenant.
 */

export class MissingResourceConfigurationsForTenant extends Error {
    constructor(tenantId: TenantId) {
        super(`Missing resource configurations for '${tenantId}'`);
    }
}
