// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { HomeProps } from "./HomeProps";

export class HomeViewModel {
    counter: string = '';


    attached() {
        console.log('attached');
    }

    propsChanged(props: HomeProps) {
        console.log(props.something);
        this.counter = props.something;
    }
}
