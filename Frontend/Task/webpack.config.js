const webpack = require('webpack');
const path = require('path');

const webpackConfig = {
    plugins: [
        new webpack.NoErrorsPlugin(),
    ],
    entry: {
        app: './app/index.js',
    },
    output: {
	      path: path.resolve(__dirname, 'dist'),
	      filename: 'bundle.js',
        library: 'app',
        libraryTarget: 'window',
    },
    resolve: {
        extensions: ['', '.js', '.json'],
    },
    module: {
        loaders: [{
	        test: /\.(js|jsx)$/,
            exclude: /(node_modules|Generated)/,
            loader: 'babel-loader',
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
