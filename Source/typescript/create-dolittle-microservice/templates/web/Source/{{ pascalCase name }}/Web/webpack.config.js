const webpack = require('@dolittle/vanir-webpack/frontend');
module.exports = (env, argv) => {
    return webpack(env, argv, '{{lowerCase uiPath}}', config => {
        config.devServer.port = 9003;
    });
};
