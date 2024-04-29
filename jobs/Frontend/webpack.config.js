const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const TerserPlugin = require('terser-webpack-plugin');
const ImageMinimizerPlugin = require('image-minimizer-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');
const { merge } = require('webpack-merge');
const Dotenv = require('dotenv-webpack');

const commonConfig = {
  entry: './src/index.tsx',
  output: {
    publicPath: '/',
    path: path.resolve(__dirname, 'dist'),
    filename: '[name].[contenthash].js',
    chunkFilename: '[name].[contenthash].js',
  },
  resolve: {
    extensions: ['.js', '.jsx', '.ts', '.tsx'],
    alias: {
      '@mui/styled-engine': '@mui/styled-engine-sc'
    },
  },
  module: {
    rules: [
      {
        test: /\.(ts|tsx)$/,
        exclude: /node_modules/,
        use: ['ts-loader'],
      },
      {
        test: /\.(png|svg|jpg|jpeg|gif)$/i,
        type: 'asset/resource',
      },
    ],
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: './public/index.html',
      filename: './index.html',
    }),
    new Dotenv(),
    new CleanWebpackPlugin(),
    new CopyWebpackPlugin({
      patterns: [
        {
          from: path.resolve(__dirname, 'public'),
          globOptions: {
            ignore: ['**/index.html'],
          },
          noErrorOnMissing: true
        },
      ],
    }),
  ],
};

const developmentConfig = {
  devtool: 'inline-source-map',
  devServer: {
    historyApiFallback: true,
    port: 3030,
  },
  optimization: {
    splitChunks: {
      chunks: 'all',
    },
  },
};

const productionConfig = {
  plugins: [
    new ImageMinimizerPlugin({
      minimizer: {
        implementation: ImageMinimizerPlugin.imageminMinify,
        options: {
          plugins: [['optipng', { optimizationLevel: 5 }]],
        },
      },
    }),
  ],
  optimization: {
    minimize: true,
    minimizer: [new TerserPlugin()],
    splitChunks: {
      cacheGroups: {
        commons: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendors',
          chunks: 'all',
        },
      },
    },
  },
};

module.exports = (_, args) => {
  switch (args.mode) {
    case 'development':
      return merge(commonConfig, developmentConfig);
    case 'production':
      return merge(commonConfig, productionConfig);
    default:
      throw new Error('No matching configuration was found!');
  }
};
