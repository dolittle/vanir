// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { a_view_model_lifecycle_manager } from './given/a_view_model_lifecycle_manager';

import sinon from 'sinon';

class ViewModel {
}

describe('when signalling params changed for view model with props changed method', () => {
    const given = new a_view_model_lifecycle_manager();
    const viewModel = {
        paramsChanged: sinon.stub()
    };

    const params = {
        something: 42
    };

    let error: Error;

    try {
        given.manager.paramsChanged(viewModel, params);
    } catch (ex) {
        error = ex;
    }

    it('should call the view models params changed', () => viewModel.paramsChanged.should.be.calledOnceWith(params));
});
