const path = require('path');
const CleanWebpackPlugin = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const WebpackNotifierPlugin = require('webpack-notifier');
const webpack = require('webpack');

const JSON = require('../package');
const projectName = JSON.name;

const devMode = process.env.NODE_ENV !== 'production';

module.exports = {
  entry: {
    app: './app/react/index.js',
    polyfills: './app/react/polyfills.js'
  },
  module: {
    rules: [
      {
        test: /\.(c|sa|sc)ss$/,
        use: [
          devMode ? 'style-loader' : MiniCssExtractPlugin.loader,
          {loader: 'css-loader', options: { sourceMap: true }},
          {loader: 'sass-loader', options: { sourceMap: true }}
        ]
      },
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: ['babel-loader']
      },
      {
        test: /\.(png|svg|jpg|gif)$/,
        use: ['file-loader']
      },
      // {
      //   test: /\.(woff|woff2|eot|ttf|otf)$/,
      //   use: ['file-loader']
      // }
    ]
  },
  plugins: [
    new CleanWebpackPlugin(),
    new HtmlWebpackPlugin({
      title: projectName,
      hash: true,
      template: path.resolve(__dirname, '..', 'app', 'index.html')
    }),
    new webpack.HotModuleReplacementPlugin(),
    new MiniCssExtractPlugin({
      filename: devMode ? 'style.css' : 'style.[hash].css',
      chunkFilename: devMode ? '[id].css' : '[id].[hash].css'
    }),
    new WebpackNotifierPlugin({ alwaysNotify: true })
  ],
  output: {
    filename: '[name].js',
    path: path.resolve(__dirname, 'dist'),
    publicPath: '/'
  },
  // resolve: {
  //   alias: {
  //     'img': path.join(__dirname, 'src', 'assets', 'images')
  //   }
  // }
};
