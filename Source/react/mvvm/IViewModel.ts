// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ICanBeNotifiedWhenRouteChanged } from './ICanBeNotifiedWhenRouteChanged';
import { ICanBeNotifiedWhenPropsChanged } from './ICanBeNotifiedWhenPropsChanged';
import { ICanBeNotifiedWhenBeingDetached } from './ICanBeNotifiedWhenBeingDetached';
import { ICanBeNotifiedWhenBeingAttached } from './ICanBeNotifiedWhenBeingAttached';

export interface IViewModel extends ICanBeNotifiedWhenBeingAttached, ICanBeNotifiedWhenBeingDetached, ICanBeNotifiedWhenPropsChanged, ICanBeNotifiedWhenRouteChanged {
}
