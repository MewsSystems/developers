"use strict";

const debug = process.env.NODE_ENV !== "production";

const webpack = require('webpack');
const path = require('path');

module.exports = {
    devtool: debug ? 'inline-sourcemap' : null,
    entry: path.join(__dirname, 'app', 'app-client.js'),
    devServer: {
        inline: true,
        port: 8080,
        contentBase: "app/static/",
        historyApiFallback: {
            index: '/index.html'
        }
    },
    output: {
        path: path.join(__dirname, 'app', 'static', 'js'),
        publicPath: "/js/",
        filename: 'app.js'
    },
    module: {
        loaders: [
            {
                test: /\.js/,
                loader: ['babel-loader'],
                query: {
                    cacheDirectory: 'babel_cache',
                    presets: debug ? ['react', 'es2015', 'react-hmre'] : ['react', 'es2015']
                }
            },
            { test: /\.json$/, loader: 'json' },
            {
                test: /\.css/,
                loaders: ['style', 'css'],
            }
        ]
    },
    plugins: debug ? [] : [
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': JSON.stringify(process.env.NODE_ENV)
            }),
            new webpack.optimize.DedupePlugin(),
            new webpack.optimize.OccurenceOrderPlugin(),
            new webpack.optimize.UglifyJsPlugin({
                compress: { warnings: false },
                mangle: true,
                sourcemap: false,
                beautify: false,
                dead_code: true
            }),
        ]
};
