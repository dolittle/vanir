// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { HomeProps } from "./HomeProps";
import { RouteInfo } from '@dolittle/vanir-react';
import { injectable } from 'tsyringe';
import { INavigator } from '@dolittle/vanir-web';

@injectable()
export class HomeViewModel {
    counter: string = '';

    constructor(private readonly _navigator: INavigator) {
    }

    attached(routeInfo: RouteInfo) {
        console.log('attached with ', routeInfo);
    }

    propsChanged(props: HomeProps) {
        this.counter = props.something;
    }

    routeChanged(routeInfo: RouteInfo) {
        console.log(routeInfo);
    }

    goAway() {
        this._navigator.navigateTo('/blah/50');
    }
}
