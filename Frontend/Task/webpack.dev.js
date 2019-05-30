// const webpack = require('webpack');
//
// const webpackConfig = {
//   plugins: [
//     new webpack.NoErrorsPlugin(),
//   ],
//   entry: {
//     app: './app/app.js',
//   },
//   output: {
//     filename: '[name].js',
//     library: 'app',
//     libraryTarget: 'window',
//   },
//   resolve: {
//     extensions: ['', '.js', '.json'],
//   },
//   module: {
//     loaders: [{
//       test: /\.js?$/,
//       exclude: /(node_modules|Generated)/,
//       loader: 'babel',
//     }, {
//       test: /\.json$/,
//       loader: 'json',
//     }, {
//       test: /\.(c|sa|sc)ss$/,
//       loader:
//     }],
//   },
//   devtool: 'eval',
//   devServer: {
//     contentBase: './app',
//   },
// };
//
// module.exports = webpackConfig;


const merge = require('webpack-merge');
const common = require('./webpack/webpack.common');
const path = require('path');
// const devServer = require('./webpack/webpack.devServer');

module.exports = merge(common, {
  mode: 'development',
  devtool: 'inline-source-map',
  devServer: {
    contentBase: path.resolve(__dirname, 'dist'),
    historyApiFallback: false,
    host: 'localhost',
    hot: true,
    inline: true,
    port: 8080,
    proxy: {
      '**': 'http://localhost:3000',
    }
  }
});

