// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { injectable } from 'tsyringe';
import { INavigator } from '@dolittle/vanir-web';

@injectable()
export class HomeViewModel {
    counter: string = '';

    constructor(private readonly _navigator: INavigator) {
    }

    goAway() {
        this._navigator.navigateTo('/blah/50');
    }
}
