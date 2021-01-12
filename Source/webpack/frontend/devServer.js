// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

const fileTypes = /\.(js|css|html|png|jpg|jpeg|gif)$/;

module.exports = (basePath, port) => {
    const port = process.env.port || port || 9000;

    return {
        historyApiFallback: { index: basePath },
        host: '0.0.0.0',
        port: port,
        publicPath: basePath,
        contentBase: process.cwd(),
        proxy: {
            '/api': 'http://localhost:3000',
            '/graphql': 'http://localhost:3000',
        },
        before: (app, server, compiler) => {
            app.get('*', (req, res, next) => {
                const match = req.originalUrl.match(fileTypes);
                if (match && match.length > 0) {
                    next();
                    return;
                }
                const html = require('./HtmlInterceptorPlugin').getGeneratedHtml();
                res.send(html);
            });
        }
    };
};
