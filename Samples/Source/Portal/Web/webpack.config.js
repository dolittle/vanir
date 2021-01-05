const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '/', config => {
        config.devServer.port = process.env.port || 9000;
        config.devServer.proxy = {
            '//graphql': 'http://localhost:3000',
            '/api': 'http://localhost:3000'
        };
    }, 'SampleApp');
};
