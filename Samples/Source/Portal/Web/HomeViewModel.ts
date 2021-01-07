// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { HomeProps } from "./HomeProps";
import { RouteInfo } from '@dolittle/vanir-react';

export class HomeViewModel {
    counter: string = '';


    attached(routeInfo: RouteInfo) {
        console.log('attached with ', routeInfo);
    }

    propsChanged(props: HomeProps) {
        //console.log(props.something);
        this.counter = props.something;
    }

    routeChanged(routeInfo: RouteInfo) {
        console.log(routeInfo);
    }
}
