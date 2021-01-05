// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { constructor } from '@dolittle/vanir-dependency-inversion';
import { container } from 'tsyringe';
import { IViewModelLifecycleManager } from './IViewModelLifecycleManager';
import { ViewModelLifecycleManager } from './ViewModelLifecycleManager';

export class Bindings {
    static initialize() {
        container.registerSingleton(IViewModelLifecycleManager as constructor<IViewModelLifecycleManager>, ViewModelLifecycleManager);
    }
}
