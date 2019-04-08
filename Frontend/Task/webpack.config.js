const webpack = require('webpack');

const webpackConfig = {
    // plugins: [
    //     new webpack.NoErrorsPlugin(),
    // ],
    entry: ['@babel/polyfill', './app/js/index.tsx'],
    output: {
        filename: 'app.js'
    },
    module: {
        rules: [{
            test: /\.(js|ts)x?$/,
            exclude: /(node_modules|Generated)/,
            loader: 'babel-loader',
        }, {
            test: /\.json$/,
            loader: 'json',
        }],
    },
    devtool: 'source-map',
    devServer: {
        contentBase: './app',
    },
    resolve: {
        extensions: ['.js', '.json', '.ts', '.tsx'],
    }
};

module.exports = webpackConfig;
