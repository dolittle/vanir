// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { a_view_model_lifecycle_manager } from './given/a_view_model_lifecycle_manager';

import sinon from 'sinon';
import { RouteInfo } from '../RouteInfo';

class ViewModel {
}

describe('when signalling attached for view model with attached method', () => {
    const given = new a_view_model_lifecycle_manager();

    let receivedRouteInfo: RouteInfo = { url: '', matchedUrl: '', isExactMatch: false, route: '', params: { myParam: 0 } };

    const viewModel = {
        attached: sinon.fake((r) => receivedRouteInfo = r)
    };

    const routeInfo: RouteInfo = {
        url: '/some/42',
        matchedUrl: '/some/42',
        isExactMatch: true,
        route: '/some/path',
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
    it('should call the view models attached method with correct matched url', () => receivedRouteInfo.matchedUrl.should.equal(routeInfo.matchedUrl));
    it('should call the view models attached method with correct route', () => receivedRouteInfo.route.should.equal(routeInfo.route));
    it('should call the view models attached method with correct params', () => (receivedRouteInfo.params as any).myParam.should.equal((routeInfo.params as any).myParam));
});
