// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Bindings } from './Bindings';
import { Bindings as WebBindings } from '@dolittle/vanir-web/Bindings';
import { Bindings as MVVMBindings } from './mvvm/Bindings';
import { Configuration } from '@dolittle/vanir-web/Configuration';

export * from './Bootstrapper';
export * from './mvvm';
export * from './routing';
export * from './MicroserviceContext';

export function initializeFrontend(configuration: Configuration) {
    Bindings.initialize(configuration);
    WebBindings.initialize(configuration);
    MVVMBindings.initialize();
}

