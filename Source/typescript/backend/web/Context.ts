// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import cls from 'cls-hooked';
import { Request, Response, NextFunction } from 'express';
import { TenantId } from '@dolittle/sdk.execution';

const ContextKey = 'Context';
const ns = cls.createNamespace('0e852ce9-ef09-45d2-b64b-4868de226563');

export class Context {
    userId: string = '';
    tenantId: TenantId = TenantId.development;
    cookies: string = '';

    static fromRequest(req: Request): Context {
        const tenantId = req.header('Tenant-ID') || TenantId.development;

        const context: Context = {
            userId: req.header('User-ID') || '',
            tenantId: TenantId.from(tenantId),
            cookies: req.header('Cookie') || ''
        };

        return context;
    }
};


export function ContextMiddleware(req: Request, res: Response, next: NextFunction) {
    ns.run(() => {
        const context = Context.fromRequest(req);

        if (ns.active) {
            ns.set(ContextKey, context);
        }
        next();
    });
}

export function getCurrentContext(): Context {
    if (ns.active) {
        return ns.get(ContextKey) as Context;
    }
    return {
        userId: '',
        tenantId: TenantId.development,
        cookies: ''
    };
}
