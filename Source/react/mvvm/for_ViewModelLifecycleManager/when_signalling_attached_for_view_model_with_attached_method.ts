// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { a_view_model_lifecycle_manager } from './given/a_view_model_lifecycle_manager';

import sinon from 'sinon';
import { RouteInfo } from '../RouteInfo';

class ViewModel {
}

describe('when signalling attached for view model with attached method', () => {
    const given = new a_view_model_lifecycle_manager();

    let receivedRouteInfo: RouteInfo = { url: '', path: '', params: { myParam: 0 } };

    const viewModel = {
        attached: sinon.fake((r) => receivedRouteInfo = r)
    };

    const routeInfo = {
        url: 'http://somewhere',
        path: '/some/path',
        params: {
            myParam: 42
        }
    };

    let error: Error;

    try {
        given.manager.attached(viewModel, routeInfo);
    } catch (ex) {
        error = ex;
    }

    it('should call the view models attached method', () => viewModel.attached.should.be.calledOnce);
    it('should call the view models attached method with correct url', () => receivedRouteInfo.url.should.equal(routeInfo.url));
    it('should call the view models attached method with correct path', () => receivedRouteInfo.path.should.equal(routeInfo.path));
    it('should call the view models attached method with correct params', () => (receivedRouteInfo.params as any).myParam.should.equal(routeInfo.params.myParam));
});
