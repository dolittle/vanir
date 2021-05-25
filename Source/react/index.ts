// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Bindings } from './Bindings';
import { Bindings as WebBindings } from '@dolittle/vanir-web';
import { Bindings as MVVMBindings } from './mvvm/Bindings';
import { Configuration } from '@dolittle/vanir-web/Configuration';
import * as DependencyInversion from '@dolittle/vanir-dependency-inversion';

export * from './Bootstrapper';
export * from './mvvm';
export * from './routing';
export * from './MicroserviceContext';

export * from './useDialog';
export * from './features';

export * as mvvm from './mvvm';
export * as routing from './routing';

export function initializeFrontend(configuration: Configuration) {
    DependencyInversion.initialize();
    Bindings.initialize(configuration);
    WebBindings.initialize(configuration);
    MVVMBindings.initialize();
}
