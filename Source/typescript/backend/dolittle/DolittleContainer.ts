// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IContainer } from '@dolittle/sdk.common';
import { Constructor } from '@dolittle/types';
import { ExecutionContext } from '@dolittle/sdk.execution';
import { IContainer as VanirContainer } from '@dolittle/vanir-dependency-inversion';
import { setCurrentContextFromExecutionContext } from '../Context';

export class DolittleContainer implements IContainer {

    constructor(private readonly _innerContainer: VanirContainer) {
    }

    get(service: Constructor, executionContext: ExecutionContext): any {
        setCurrentContextFromExecutionContext(executionContext);
        return this._innerContainer.get(service);
    }
}
