const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '/_/typescript', config => {
        config.devServer.proxy = {
            '/_/typescript/graphql': 'http://localhost:3003',
            '/api': 'http://localhost:3003'
        };
    }, 9003, 'SampleApp');
};
