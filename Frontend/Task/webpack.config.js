const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');

const webpackConfig = {
  plugins: [
    new HtmlWebpackPlugin({
      template: './app/index.html',
    }),
  ],
  entry: {
    app: './app/index.js',
  },
  output: {
    filename: '[name].js',
    path: path.join(__dirname, '/build'),
    library: 'app',
    libraryTarget: 'window',
  },
  module: {
    rules: [
      {
        test: /\.js?$/,
        exclude: /(node_modules)/,
        use: [
          'thread-loader',
          'babel-loader?cacheDirectory',
        ],
      },
      {
        test: /\.scss?$/,
        use: [
          'style-loader',
          {
            loader: 'css-loader',
            options: {
              modules: true
            }
          },
          'sass-loader',
        ],
      },
    ],
  },
  devtool: 'eval',
  devServer: {
    contentBase: './app',
  },
};

module.exports = webpackConfig;
