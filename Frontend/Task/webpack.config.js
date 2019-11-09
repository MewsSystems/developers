const webpack = require('webpack');

const webpackConfig = {
    plugins: [
        new webpack.NoErrorsPlugin(),
    ],
    entry: {
        app: './app/app.js',
    },
    output: {
        filename: '[name].js',
        library: 'app',
        libraryTarget: 'window',
    },
    resolve: {
        extensions: ['', '.js', '.jsx', '.json'],
    },
    module: {
        loaders: [{
            test: /\.jsx?$/,
            exclude: /(node_modules|Generated)/,
            loader: 'babel',
        }, {
            test: /\.json$/,
            loader: 'json',
        }],
    },
    devtool: 'eval',
    devServer: {
        contentBase: './app',
    },
};

module.exports = webpackConfig;
