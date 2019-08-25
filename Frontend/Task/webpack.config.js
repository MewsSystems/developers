const webpack = require('webpack');
const path = require('path');

const webpackConfig = {
  plugins: [new webpack.NoErrorsPlugin()],
  entry: {
    app: './app/app.js'
  },
  output: {
    filename: '[name].js',
    library: 'app',
    libraryTarget: 'window'
  },
  resolve: {
    alias: {
      App: path.resolve(__dirname, 'app'),
      Actions: path.resolve(__dirname, 'app/actions'),
      API: path.resolve(__dirname, 'app/api'),
      Components: path.resolve(__dirname, 'app/components'),
      Containers: path.resolve(__dirname, 'app/containers'),
      Reducers: path.resolve(__dirname, 'app/reducers'),
      Selectors: path.resolve(__dirname, 'app/selectors'),
      Utils: path.resolve(__dirname, 'app/utils')
    },
    extensions: ['', '.js', '.json']
  },
  module: {
    loaders: [
      {
        test: /\.css$/,
        loaders: ['style-loader', 'css-loader?modules=true&camelCase=true']
      },
      {
        test: /\.js?$/,
        exclude: /(node_modules|Generated)/,
        loader: 'babel'
      },
      {
        test: /\.json$/,
        loader: 'json'
      }
    ]
  },
  devtool: 'eval',
  devServer: {
    contentBase: './app'
  }
};

module.exports = webpackConfig;
