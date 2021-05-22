// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

const path = require('path');

const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '/', config => {
        config.output.path = path.join(process.cwd(),'../vscode/dist/Visual');
        config.output.filename = 'index.js';
        config.output.sourceMapFilename = 'index.map';
        config.output.chunkFilename = 'app.js';
        config.output.chunkFilename = 'main.js';
        config.optimization.runtimeChunk = false;
        config.optimization.splitChunks = undefined;
        config.devServer.proxy = {
        };
    }, 9100, 'Microservices Editor');
};
