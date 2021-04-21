// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { MiddlewareInterface, NextFn, ResolverData } from 'type-graphql';
import { BrokenRuleError } from '../aggregates/BrokenRule';

export class BrokenRuleErrorInterceptor implements MiddlewareInterface {
    async use({ context, info }: ResolverData<any>, next: NextFn) {
        try {
            return await next();
        } catch (err) {
            if (err instanceof BrokenRuleError) {
                throw err;
            } else {
                throw err;
            }
        }
    }
}
