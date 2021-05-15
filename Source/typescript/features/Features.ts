// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IFeatureToggleStrategy } from './IFeatureToggleStrategy';

/**
 * Represents a set of {@link Features}.
 */
export class Features extends Map<string, IFeatureToggleStrategy> {
}
