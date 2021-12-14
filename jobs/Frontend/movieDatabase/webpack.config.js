const HtmlWebpackPlugin = require('html-webpack-plugin');

const path = require('path');

const basePath = __dirname;

module.exports = {
    mode: 'production',
    context: path.join(basePath,'src'),
    entry: './index.tsx',
    output: {
        publicPath: '',
        path: __dirname + '/dist',
        filename: 'bundle.js'
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js", ".css", ".less"],
    },
    devServer: {
        static: {
            directory: path.join(__dirname, 'src'),
        },
        compress: true,
        port: 9000,
    },
    module: {
        rules: [
            {
                test: /\.(ts|tsx)$/,
                use: [
                    {
                        loader: "ts-loader",
                        options: {
                            configFile: "../tsconfig.json",
                        }
                    }
                ]
            },
            {
                test: /\.(less)$/,
                use: [{
                    loader: 'style-loader'
                }, {
                    loader: 'css-loader'
                }, {
                    loader: 'less-loader'
                }]
            },
            {
                test: /\.css$/,
                use: [
                    { loader: "style-loader" },
                    { loader: "css-loader" }
                ]
            },
        ]
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: './index.html'
        })
    ]
};