const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');
const path = require('path');

module.exports = {
    mode: 'development',
    entry: [
        './app/app.js'
    ],
    output: {
        path: path.resolve(__dirname, 'dist'),
        publicPath: '/',
        filename: 'bundle.js'
    },
    module : {
        rules : [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: [
                    'babel-loader',
                ]
            },
            {
                test: /\.jsx?$/,         // Match both .js and .jsx files
                exclude: /node_modules/,
                loader: "babel-loader",
                query:
                {
                    presets:['react']
                }
            },
            {
                test: /\.tsx?$/,
                exclude: /node_modules/,
                use: ['babel-loader', {loader: 'ts-loader',   options: {configFile:  'app/tsconfig.json'}}]
            }

        ]
    },
    resolve: {
        extensions: ['*', '.js', '.jsx', '.ts', '.tsx'],
        alias: {
            '@services':    path.resolve(__dirname, 'app/services'),
            '@components':  path.resolve(__dirname, 'app/components'),
            '@ui':          path.resolve(__dirname, 'app/components/ui'),
            '@constants':   path.resolve(__dirname, 'app/constants'),
            '@utils':       path.resolve(__dirname, 'app/utils'),
            '@store':       path.resolve(__dirname, 'app/store'),
            '@icons':       path.resolve(__dirname, 'app/icons')
          },
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: 'app/index.html',
            inject: 'body',
        })
    ],
    devtool: 'inline-source-map',

};