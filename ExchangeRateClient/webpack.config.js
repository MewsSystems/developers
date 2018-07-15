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
            },
            {
                test: /\.css?$/,
                loader: 'style-loader!css-loader'
            },
            {
                test: /\.scss?$/,
                loader: 'style-loader!css-loader!sass-loader',
                include: path.join(__dirname, 'app', 'components')
            },
            {
                test: /\.(ttf|eot|svg|woff(2)?)(\?[a-z0-9]+)?$/,
                loader: 'file-loader'
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
