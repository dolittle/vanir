// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Constructor } from '@dolittle/types';
import { BehaviorSubject } from 'rxjs';

const internalPrefix = '__';
const observableProperties = `${internalPrefix}observable_properties`;

export type Observables = { [key: string]: BehaviorSubject<any> };

export class ObservableProperties {
    static registerPropertyForType(constructor: Constructor, key: string): void {
        const props = constructor[observableProperties] || {};
        props[key] = undefined;
        constructor[observableProperties] = props;
    }

    static initializePropertiesFor(target: any): void {
        const observables = this.getObservablesFor(target);
        const properties = {};
        for (const property of Object.keys(observables)) {
            properties[property] = new BehaviorSubject(target[property]);

            Object.defineProperty(target, property, {
                get: () => {
                    return properties[property].value;
                },
                set: (value: any) => {
                    if (properties[property]) {
                        properties[property].next(value);
                    }
                }
            });
        }

        target[observableProperties] = properties;
    }

    static getObservablesFor(target: any): Observables {
        return target[observableProperties] || {};
    }
}
