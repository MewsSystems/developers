const webpack = require('webpack')
const path = require('path')
const ExtractTextPlugin = require('extract-text-webpack-plugin')

module.exports = {
  entry: './app/app.js',
  output: {
    filename: '[name].js',
    library: 'app',
    libraryTarget: 'window',
  },
  resolve: {
    extensions: ['.js', '.json'],
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        include: path.resolve(__dirname, 'app'),
        loader: 'babel-loader',
      },
      {
        test: /\.json$/,
        include: path.resolve(__dirname, 'app'),
        loader: 'json-loader',
      },
      {
        test: /\.css$/,
        include: path.resolve(__dirname, 'app'),
        loader: ExtractTextPlugin.extract({
          fallback: 'style-loader',
          use: [
            {
              loader: 'css-loader',
              query: {
                modules: true,
                localIdentName: '[name]__[local]___[hash:base64:5]',
              }
            },
          ],
        }),
      },
    ],
  },
  devtool: 'cheap-eval-source-map',
  devServer: {
    contentBase: './app',
  },
  plugins: [
    new webpack.NoEmitOnErrorsPlugin(),
  ],
}
