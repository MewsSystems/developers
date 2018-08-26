const webpack = require('webpack');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
var combineLoaders = require('webpack-combine-loaders');

const webpackConfig = {
    plugins: [
        new webpack.NoErrorsPlugin(),
        new ExtractTextPlugin('style.css'),
    ],
    entry: {
        app: ['babel-polyfill', './app/app.js'],
    },
    output: {
        filename: '[name].js',
        library: 'app',
        libraryTarget: 'window',
    },
    resolve: {
        extensions: ['', '.js', '.json', 'css'],
    },
    module: {
        loaders: [{
            test: /\.js?$/,
            exclude: /(node_modules|Generated)/,
            loader: 'babel',
        }, 
        {
          test: /\.css$/,
          loader: ExtractTextPlugin.extract(
            'style-loader',
            combineLoaders([{
              loader: 'css-loader',
              query: {
                modules: true,
                localIdentName: '[name]__[local]___[hash:base64:5]'
              }
            }])
            )
          },
        {
            test: /\.json$/,
            loader: 'json',
        }],
    },
    postcss: [
      require('autoprefixer-core'),
      require('postcss-color-rebeccapurple')
    ],
    devtool: 'eval',
    devServer: {
        contentBase: './app',
    },
};

module.exports = webpackConfig;
