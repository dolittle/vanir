// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Bindings } from './Bindings';
import { Bindings as MVVMBindings } from './mvvm/Bindings';
import { Configuration } from './Configuration';

export * from './Bootstrapper';
export * from './DataSource';
export * from './mvvm';
export * from './routing';
export * from './Configuration';
export * from './MicroserviceContext';

export function initializeFrontend(configuration: Configuration) {
    Bindings.initialize(configuration);
    MVVMBindings.initialize();
}

