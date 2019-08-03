const webpack = require('webpack');
const path = require('path');

const webpackConfig = {
  plugins: [new webpack.NoEmitOnErrorsPlugin()],
  mode: 'development',
  entry: {
    app: './app/app.js'
  },
  output: {
    path: path.resolve(__dirname, 'dist/'),
    publicPath: '/dist/',
    filename: 'bundle.js'
  },
  resolve: {
    extensions: ['.js', '.json']
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /(node_modules|Generated)/,
        loader: 'babel-loader'
      }
    ]
  },
  devtool: 'eval',
  devServer: {
    contentBase: './app'
  }
};

module.exports = webpackConfig;
