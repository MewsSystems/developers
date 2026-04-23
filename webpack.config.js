const path = require('path');
const webpack = require('webpack');
require('dotenv').config();

module.exports = {
  entry: path.resolve(__dirname, 'src', 'index.tsx'),
  mode: 'development', // or production
  output: {
    path: path.resolve(__dirname, 'public/assets'),
    publicPath: '/assets/',
    filename: 'bundle.js',
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx|tsx|ts)$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
        },
      },
    ],
  },
  plugins: [new webpack.EnvironmentPlugin(['NODE_ENV', 'MOVIE_SEARCH_API_KEY'])],
  resolve: {
    extensions: ['.ts', '.tsx', '.js'],
  },
  devServer: {
    historyApiFallback: true,
    static: { directory: path.join(__dirname, 'public') },
    port: 3000,
    hot: true,
    historyApiFallback: true,
  },
};
