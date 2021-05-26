// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Request, Response, NextFunction } from 'express';
import { TenantId, ExecutionContext } from '@dolittle/sdk.execution';
import { HostContext } from './HostContext';

const ContextKey = 'Context';

export class Context {
    userId = '';
    tenantId: TenantId = TenantId.development;
    cookies = '';

    static fromRequest(req: Request): Context {
        const tenantId = req.header('Tenant-ID') || TenantId.development;

        const context: Context = {
            userId: req.header('User-ID') || '',
            tenantId: TenantId.from(tenantId),
            cookies: req.header('Cookie') || ''
        };

        return context;
    }
}

export function ContextMiddleware(req: Request, res: Response, next: NextFunction) {
    const context = Context.fromRequest(req);
    HostContext.set(ContextKey, context);
    next();
}

export function setCurrentContextFromExecutionContext(executionContext: ExecutionContext) {
    HostContext.set(ContextKey, {
        tenantId: executionContext.tenantId
    } as Context);
}

export function getCurrentContext(): Context {
    return HostContext.get(ContextKey) as Context;
}
