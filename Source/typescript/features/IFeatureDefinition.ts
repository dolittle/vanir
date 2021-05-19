// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggleDefinition } from './IFeatureToggleDefinition';

export interface IFeatureDefinition {
    name: string;
    description: string;
    toggles: IFeatureToggleDefinition[];
}
