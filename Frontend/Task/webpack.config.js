'use strict';

const path = require('path');
const env = process.env.NODE_ENV || 'development';
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const Dotenv = require('dotenv-webpack');

module.exports = {
    mode: env,
    entry: './app/app.tsx',
    output: {
        path: path.resolve(__dirname, 'public'),
        filename: 'main.js',
        publicPath: '/',
        globalObject: 'this'
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: '[name].bundle.css',
            chunkFilename: '[name].chunk.css',
        }),
        new Dotenv()
    ],
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            [
                                '@babel/env',
                                {
                                    'targets': {
                                        'browsers': [
                                            '>0.25%',
                                            'not ie 11',
                                            'not op_mini all',
                                        ],
                                    },
                                },
                            ],
                            '@babel/react',
                            '@babel/typescript',
                        ],
                        plugins: [
                            '@babel/proposal-class-properties',
                            '@babel/proposal-object-rest-spread',
                            ['@babel/transform-runtime', {
                                "regenerator": true
                            }]
                        ],
                    },
                },
            },
            {
                test: /\.(sass|scss)$/,
                exclude: /node_modules/,
                loaders: [MiniCssExtractPlugin.loader, 'css-loader', 'sass-loader']
            }
        ],
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js'],
    },
    devtool: 'source-map',
    devServer: {
        contentBase: path.resolve(__dirname, 'public'),
        historyApiFallback: true,
    },
};
