const webpack = require('webpack');
const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
  entry: ['babel-polyfill','./app/index.jsx'],
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: ['babel-loader'],
      },{
          test: /\.(sass|scss|css)$/,
          exclude: /node_modules/,
          loaders: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader']
      }
    ],
  },
  resolve: {
    extensions: ['*', '.js', '.jsx'],
  },
  output: {
    path: `${__dirname}/dist`,
    publicPath: '/',
    filename: 'bundle.js',
  },
  plugins: [
		new CleanWebpackPlugin(),
		new HtmlWebpackPlugin({
      inject: 'body',
      template: './app/index.html',
    }),
		new MiniCssExtractPlugin({
        filename: '[name].bundle.css',
        chunkFilename: '[name].chunk.css',
    }),
    new webpack.HotModuleReplacementPlugin(),
  ],
};
