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
    extensions: ['.js', '.jsx', '.json'],
    modules: [path.join(__dirname, 'app'), 'node_modules'],
    alias: {
      Containers: path.resolve(paths.appSrc, 'containers'),
      Components: path.resolve(paths.appSrc, 'components'),
      Redux: path.resolve(paths.appSrc, 'redux'),
      Config: path.resolve(paths.appConfig),
    },
  },
  module: {
    rules: [
      {
        test: /\.(png|svg|jpg|gif)$/,
        use: ['file-loader'],
      },
    ],
  },
};
