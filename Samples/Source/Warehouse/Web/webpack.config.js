// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '/_/warehouse', config => {
        config.devServer.proxy = {
            '/_/warehouse/graphql': 'http://localhost:3002',
            '/api': 'http://localhost:3002'
        };
    }, 9002, 'eCommerce');
};
