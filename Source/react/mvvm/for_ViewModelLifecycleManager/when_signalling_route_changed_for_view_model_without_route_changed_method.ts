// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { a_view_model_lifecycle_manager } from './given/a_view_model_lifecycle_manager';
import { expect } from 'chai';

class ViewModel {
}

describe('when signalling route changed for view model without route changed method', () => {
    const given = new a_view_model_lifecycle_manager();
    const viewModel = {
    };

    const routeInfo = {
        url: '/some/42',
        urlMatch: '/some/42',
        path: '/some/:id',
        params: {
            something: 42
        }
    };

    let error: Error;

    try {
        given.manager.routeChanged(viewModel, routeInfo);
    } catch (ex) {
        error = ex;
    }

    it('should not throw an exception', () => expect(error).to.be.undefined);
});
