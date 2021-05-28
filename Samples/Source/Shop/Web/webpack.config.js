// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '/_/shop', config => {
        config.devServer.proxy = {
            '/_/shop/graphql': 'http://localhost:3003',
            '/api': 'http://localhost:3003'
        };
    }, 9003, 'SampleApp');
};
