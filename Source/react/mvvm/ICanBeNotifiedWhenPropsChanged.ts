// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


/**
 * Interface for defining the signature of the props changed method that can be implemented and get called
 * during lifecycle of a view model
 */
export interface ICanBeNotifiedWhenPropsChanged {
    propsChanged<TProps>(props: TProps);
}
