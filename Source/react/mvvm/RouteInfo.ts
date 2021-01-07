// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

export type RouteInfo<TParams = {}> = {
    url: string;
    matchedUrl: string;
    route: string;
    params: TParams;
};
