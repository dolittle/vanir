// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
export class Message {
    source: string = 'unknown';
    type!: Function;
    content: any;
}
