// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { constructor } from '@dolittle/vanir-dependency-inversion';
import { IMessenger } from './IMessenger';
import { Messenger } from './Messenger';
import { container } from 'tsyringe';

export class Bindings {
    static initialize() {
        container.registerSingleton(IMessenger as constructor<IMessenger>, Messenger);
    }
}

