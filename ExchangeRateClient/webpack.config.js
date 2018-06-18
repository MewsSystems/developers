const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');
const path = require('path');

module.exports = {
    mode: 'development',
    entry: [
        './app/app.js',
    ],
    output: {
        path: path.resolve(__dirname, 'dist'),
        publicPath: '/',    
        filename: 'app.js'
    },
    module : {
        rules : [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: [
                    'babel-loader',
                ]
            }
        ]
    },
    resolve: {
        extensions: ['*', '.js', '.jsx']
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: 'app/index.html',
            inject: 'body',
        }),
        new webpack.NamedModulesPlugin(),
        //new webpack.HotModuleReplacementPlugin()
    ],
    devtool: 'eval',
};
