const path = require('path');

const webpack = require('webpack');
const merge = require('webpack-merge');
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
const ExtractTextPlugin = require('extract-text-webpack-plugin');

const paths = require('../paths');
const common = require('./webpack-common-config.js');

module.exports = merge(common, {
  entry: {
    vendor: ['redux', 'react', 'react-redux', 'react-dom'],
    app: paths.appIndexJs,
  },
  mode: 'production',
  output: {
    filename: '[chunkhash]_[name].js',
  },
  plugins: [
    new UglifyJSPlugin(),
    // set process.env.NODE_ENV to production
    new webpack.DefinePlugin({
      'process.env': {
        NODE_ENV: JSON.stringify('production'),
      },
    }),
    new ExtractTextPlugin('styles.css'),
  ],
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        include: path.resolve(paths.appSrc),
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: ['react'],
          },
        },
      },
      {
        test: /\.(css|scss)$/,
        use: ExtractTextPlugin.extract({
          fallback: 'style-loader',
          use: [
            {
              loader: 'css-loader',
              options: {
                discardDuplicates: true,
                sourceMap: false,
                modules: true,
                localIdentName: '[name]__[local]___[hash:base64:5]',
              },
            },
            {
              loader: 'sass-loader',
              options: {
                includePaths: ['app'],
                sourceMap: false,
              },
            },
          ],
        }),
      },
    ],
  },
});
