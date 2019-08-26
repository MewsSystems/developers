const webpack = require('webpack');
const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
  entry: ['babel-polyfill','./src/index.jsx'],
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: ['babel-loader'],
      }, {
         test:/\.css$/,
				exclude: /node_modules/,
        use:['style-loader','css-loader']
      },
    ],
  },
  resolve: {
    extensions: ['*', '.js', '.jsx','.css'],
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
      template: './src/index.html',
    }),
    new webpack.HotModuleReplacementPlugin(),
  ],
};
