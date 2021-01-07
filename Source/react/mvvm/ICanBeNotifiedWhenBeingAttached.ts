// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RouteInfo } from './RouteInfo';

/**
 * Interface for defining the signature of the attached method that can be implemented and get called
 * during lifecycle of a view model
 */
export interface ICanBeNotifiedWhenBeingAttached {
    attached<TParams = {}>(routeInfo: RouteInfo<TParams>);
}
