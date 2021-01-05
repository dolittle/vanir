// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { a_view_model_lifecycle_manager } from './given/a_view_model_lifecycle_manager';

import sinon from 'sinon';
import { RouteInfo } from '../RouteInfo';

class ViewModel {
}

describe('when signalling params changed for view model with params changed method', () => {
    const given = new a_view_model_lifecycle_manager();

    let receivedParams: any;

    let receivedRouteInfo: RouteInfo = {url: '', path: '', params: {}};

    const viewModel = {
        paramsChanged: sinon.fake((p, r) => {
            receivedParams = p;
            receivedRouteInfo = r;
        })
    };

    const params = {
        something: 42
    };

    const routeInfo = {
        url: 'http://somewhere',
        path: '/some/path',
        params
    };

    let error: Error;

    try {
        given.manager.paramsChanged(viewModel, params, routeInfo);
    } catch (ex) {
        error = ex;
    }

    it('should call the view models params changed', () => viewModel.paramsChanged.should.be.calledOnceWith(params));
    it('should call the view models attached method with param', () => receivedParams.something.should.equal(params.something));
    it('should call the view models attached method with correct url', () => receivedRouteInfo.url.should.equal(routeInfo.url));
    it('should call the view models attached method with correct path', () => receivedRouteInfo.path.should.equal(routeInfo.path));
    it('should call the view models attached method with correct params', () => receivedRouteInfo.params.something.should.equal(routeInfo.params.something));
});
