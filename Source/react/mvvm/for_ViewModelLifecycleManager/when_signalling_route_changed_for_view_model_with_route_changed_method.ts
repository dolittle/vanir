// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { a_view_model_lifecycle_manager } from './given/a_view_model_lifecycle_manager';

import sinon from 'sinon';
import { RouteInfo } from '../RouteInfo';

class ViewModel {
}

describe('when signalling route changed for view model with route changed method', () => {
    const given = new a_view_model_lifecycle_manager();

    let receivedRouteInfo: RouteInfo = { url: '', path: '', params: { something: 0 } };

    const viewModel = {
        routeChanged: sinon.fake((r) => {
            receivedRouteInfo = r;
        })
    };

    const routeInfo = {
        url: 'http://somewhere',
        path: '/some/path',
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

    it('should call the view models route changed', () => viewModel.routeChanged.should.be.calledOnceWith(routeInfo));
    it('should call the view models attached method with correct url', () => receivedRouteInfo.url.should.equal(routeInfo.url));
    it('should call the view models attached method with correct path', () => receivedRouteInfo.path.should.equal(routeInfo.path));
    it('should call the view models attached method with correct params', () => (receivedRouteInfo.params as any).something.should.equal(routeInfo.params.something));
});
