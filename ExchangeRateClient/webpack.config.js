const webpack = require('webpack')
const path = require('path')
const ExtractTextPlugin = require('extract-text-webpack-plugin')

module.exports = {
  entry: './src/index.js',
  output: {
    filename: './dist/[name].js',
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
        include: path.resolve(__dirname, 'src'),
        loader: 'babel-loader',
      },
      {
        test: /\.json$/,
        include: path.resolve(__dirname, 'src'),
        loader: 'json-loader',
      },
      {
        test: /\.css$/,
        include: path.resolve(__dirname, 'src'),
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
    contentBase: path.join(__dirname, "src"),
  },
  plugins: [
    new webpack.NoEmitOnErrorsPlugin(),
  ],
}
