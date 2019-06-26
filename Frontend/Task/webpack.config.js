/* eslint-disable no-undef */
const HtmlWebpackPlugin = require('html-webpack-plugin');

const webpackConfig = {
    plugins: [
        new HtmlWebpackPlugin({ template: './app/index.html' })
    ],
    entry: {
        app: './app/App.jsx',
    },
    output: {
        filename: '[name].js',
        library: 'app',
        libraryTarget: 'window',
    },
    resolve: {
        extensions: ['.js', '.jsx', '.json'],
    },
    module: {
        rules: [{
            test: /\.(js|jsx)$/,
            exclude: /(node_modules|Generated)/,
            loader: 'babel-loader',
        },
        {
            test: /\.scss$/,
            use: [
                "style-loader",
                "css-loader",
                "sass-loader"
            ]
        },
        {
            test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
            use: [{
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]',
                    outputPath: 'fonts/'
                }
            }]
        }],
    },
    devtool: 'eval',
    devServer: {
        contentBase: './app',
        proxy: {
            '*': 'http://localhost:3000'
        }
    },
};

module.exports = webpackConfig;
