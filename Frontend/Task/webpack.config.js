const webpack = require('webpack');
const path = require('path');
const CopyPlugin = require('copy-webpack-plugin');

const webpackConfig = {
    plugins: [
        new webpack.NoEmitOnErrorsPlugin(),
        new CopyPlugin([
            { from: './app/index.html', to: './dist/index.html' },
        ]),
    ],
    entry: {
        app: './app/app.tsx',
    },
    output: {
        filename: '[name].js',
        library: 'app',
        libraryTarget: 'window',
        path: path.resolve(__dirname, 'dist'),
    },
    resolve: {
        extensions: ['.js', '.json', '.ts', '.tsx'],
    },
    module: {
        rules: [{
            test: /\.(js|jsx)$/,
            exclude: /(node_modules)/,
            loader: 'babel-loader',
        }, {
            test: /\.(ts|tsx)$/,
            exclude: /(node_modules)/,
            loader: ['babel-loader', 'awesome-typescript-loader'],
        }, {
            enforce: 'pre',
            test: /\.js$/,
            loader: 'source-map-loader',
        }],
    },
    devtool: 'eval',
    devServer: {
        contentBase: path.resolve(__dirname, 'app'),
    },
};

module.exports = webpackConfig;
