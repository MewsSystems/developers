const path = require('path');
const webpack = require('webpack');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

// @todo Hot loader only for dev-server
// @todo Extract css to separated file
// @todo Analyze prod build to find excessive code

module.exports = (env = 'development') => {
    const isDevelopment = env === 'development';

    return {
        resolve: {
            alias: isDevelopment ? {
                'react-dom': '@hot-loader/react-dom',
            } : {},
            symlinks: false,
            modules: ['node_modules', 'src'],
        },
        mode: isDevelopment ? 'development' : 'production',
        entry: './src/index.tsx',
        optimization: {
            splitChunks: {
                chunks: 'all',
            },
        },
        devServer: {
            historyApiFallback: true,
            contentBase: './',
            hot: true,
            publicPath: '/',
        },
        devtool: 'source-map',
        output: {
            path: path.resolve(__dirname, 'build'),
            filename: 'js/app.[hash].js',
            sourceMapFilename: '[file].map',
        },
        module: {
            rules: [
                {
                    test: /\.(j|t)sx?$/,
                    exclude: [
                        /node_modules/,
                    ],
                    resolve: {
                        extensions: [ '.js', '.ts', '.jsx', '.tsx' ],
                    },
                    use: {
                        loader: 'babel-loader',
                        options: {
                            envName: env,
                        },
                    },
                },
            ],
        },
        watchOptions: {
            ignored: /node_modules/,
        },
        plugins: [
            new HtmlWebpackPlugin({
                template: './src/assets/index.html',
                baseUrl: '/',
            }),
            new webpack.DefinePlugin({
                'process.env': {
                    NODE_ENV: JSON.stringify(env),
                },
            }),
            new CleanWebpackPlugin({
                cleanOnceBeforeBuildPatterns: ['build'],
            }),
            ...(isDevelopment ? [
                new webpack.HotModuleReplacementPlugin(),
            ] : []),
        ],
    };
};
