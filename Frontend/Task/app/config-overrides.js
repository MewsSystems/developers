const rewireTypingsForCssModule = require("react-app-rewire-typings-for-css-module");

module.exports = {
  webpack: function(config, env) {
    /**
     * Add this line
     */
    config = rewireTypingsForCssModule(config);
    return config;
  }
};
