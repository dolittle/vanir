// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { a_view_model_lifecycle_manager } from './given/a_view_model_lifecycle_manager';
import { expect } from 'chai';

class ViewModel {
}

describe('when signalling props changed for view model without props changed method', () => {
    const given = new a_view_model_lifecycle_manager();
    const viewModel = {
    };

    const props = {
        something: 42
    };

    let error: Error;

    try {
        given.manager.propsChanged(viewModel, props);
    } catch (ex) {
        error = ex;
    }

    it('should not throw an exception', () => expect(error).to.be.undefined);
});
