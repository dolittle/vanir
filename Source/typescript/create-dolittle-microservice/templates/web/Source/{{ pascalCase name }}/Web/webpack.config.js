const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '{{lowerCase uiPath}}', config => {
        config.devServer.port = process.env.port || 9000;
        config.devServer.proxy = {
            '{{lowerCase uiPath}}/graphql': 'http://localhost:3000',
            '/api': 'http://localhost:3000'
        };
    }, '{{pascalCase applicationName}}');
};
