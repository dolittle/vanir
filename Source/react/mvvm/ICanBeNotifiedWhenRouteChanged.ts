// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { RouteInfo } from './RouteInfo';

/**
 * Interface for defining the signature of the route changed method that can be implemented and get called
 * during lifecycle of a view model
 */
export interface ICanBeNotifiedWhenRouteChanged {
    routeChanged<TParams = {}>(routeInfo: RouteInfo<TParams>);
}
