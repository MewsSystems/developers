const webpack = require('webpack');

const webpackConfig = {
  plugins: [
    new webpack.NoEmitOnErrorsPlugin(),
  ],
  entry: {
    app: './app/app.js',
  },
  output: {
    filename: '[name].js',
    library: 'app',
    libraryTarget: 'window',
  },
  resolve: {
    extensions: ['*', '.js', '.json'],
  },
  module: {
    rules: [{
      test: /\.js?$/,
      exclude: /(node_modules|Generated)/,
      loader: 'babel-loader',
    }],
  },
  devtool: 'eval',
  devServer: {
    contentBase: './app',
  },
};

module.exports = webpackConfig;
