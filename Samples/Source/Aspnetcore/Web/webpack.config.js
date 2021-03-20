const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '/_/aspnetcore', config => {
        config.devServer.proxy = {
            '/_/aspnetcore/graphql': 'http://localhost:3002',
            '/api': 'http://localhost:3002'
        };
    }, 9002, 'SampleApp');
};
