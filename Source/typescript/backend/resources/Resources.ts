// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { constructor } from '@dolittle/vanir-dependency-inversion';
import { container } from 'tsyringe';
import { IResourceConfigurations } from './IResourceConfigurations';
import { ResourceConfigurations } from './ResourceConfigurations';

export class Resources {
    static async initialize(): Promise<void> {
        container.registerSingleton(IResourceConfigurations as constructor<IResourceConfigurations>, ResourceConfigurations);
    }
}
