const HtmlWebpackPlugin = require('html-webpack-plugin');
const path = require('path');

const paths = require('../paths');

module.exports = {
  plugins: [
    new HtmlWebpackPlugin({
      inject: true,
      template: paths.appHtml,
    }),
  ],
  output: {
    filename: '[name].bundle.js',
    path: paths.appBuild,
    publicPath: '/',
  },
  resolve: {
    extensions: ['.js', '.jsx', '.json', '.scss'],
    modules: [path.join(__dirname, 'app'), 'node_modules'],
    alias: {
      Components: path.resolve(paths.appSrc, 'components'),
      Config: path.resolve(paths.appConfig),
      Containers: path.resolve(paths.appSrc, 'containers'),
      Redux: path.resolve(paths.appSrc, 'redux'),
      Utils: path.resolve(paths.appSrc, 'utils'),
      Style: path.resolve(paths.appSrc, 'style'),
    },
  },
  module: {
    rules: [
      {
        test: /\.(png|svg|jpg)$/,
        use: ['file-loader'],
      },
    ],
  },
};
