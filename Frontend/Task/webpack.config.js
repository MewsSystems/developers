const HtmlWebpackPlugin = require('html-webpack-plugin');

const prod = process.argv.indexOf('-p') !== -1;
const path = require('path');

const webpackConfig = {
    entry: path.join(__dirname, '/app/app.tsx'),
    output: {
        path: path.join(__dirname, '/dist'),
        filename: '[name].js',
        library: 'app',
        libraryTarget: 'window',
    },

    // Enable sourcemaps for debugging webpack's output.
    devtool: prod ? "source-map" : "eval-source-map",

    resolve: {
        // Add '.ts' and '.tsx' as resolvable extensions.
        extensions: [".ts", ".tsx", ".js", ".json"]
    },

    module: {
        rules: [
            // All files with a '.ts' or '.tsx' extension will be handled by 'awesome-typescript-loader'.
            {
                test: /\.tsx?$/,
                loader: 'awesome-typescript-loader',
                exclude: /(node_modules|Generated|dist|server)/
            },
        ]
    },
    devServer: {
        open: true,
        overlay: true,
        port: 9876,
        clientLogLevel: "warning",
        historyApiFallback: true,
        inline: true,
        hot: true,
        contentBase: './app',
    },
    plugins: [
        new HtmlWebpackPlugin({
            template: './app/index.html'
        })
    ]
};

module.exports = webpackConfig;
