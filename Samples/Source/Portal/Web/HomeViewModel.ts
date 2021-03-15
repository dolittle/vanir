// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { HomeProps } from "./HomeProps";
import { observable, observe, RouteInfo } from '@dolittle/vanir-react';

import { BehaviorSubject, pipe } from 'rxjs';

import { PropertyAccessor, Constructor } from '@dolittle/types';
import { ObservableProperties } from '@dolittle/vanir-react/dist/mvvm/observables/ObservableProperties';


export class HomeViewModel {
    counter: string = '';

    constructor() {

    }


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


    @observable firstName: string = 'John';

    @observable lastName: string = 'Doe';

    @observe<HomeViewModel>(_ => _.firstName, _ => _.lastName)
    get fullName() {
        return `${this.firstName} ${this.fullName}`;
    }

    @observe<HomeViewModel>(_ => _.fullName)
    doStuff() {

    }

}


const h1 = new HomeViewModel();
const h2 = new HomeViewModel();
debugger;
ObservableProperties.initializePropertiesFor(h1);
ObservableProperties.initializePropertiesFor(h2);
h1.firstName = 'Kåre';

const h1Properties = ObservableProperties.getObservablesFor(h1);
const h2Properties = ObservableProperties.getObservablesFor(h2);
