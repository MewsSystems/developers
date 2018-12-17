const webpack = require('webpack');
const path = require('path');

const webpackConfig = {
    entry: {
        app: './app/js/babel/exchange-rate-client.js',
    },
    performance: {
        hints: false,
    },
    output: {
        filename: 'exchange-rate-client.min.js',
        path: path.resolve(__dirname, "./app/js")
    },
    resolve: {
        extensions: ['.js', '.json'],
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
