const webpackConfig = {
    optimization: {
        noEmitOnErrors: true,
    },
    entry: {
        app: './app/app.js',
    },
    output: {
        filename: '[name].js',
        library: 'app',
        libraryTarget: 'window',
    },
    resolve: {
        extensions: ['.js', '.json'],
    },
    module: {
        rules: [{
            test: /\.js$/,
            exclude: /node_modules/,
            use: 'babel-loader',
        }],
    },
    devtool: 'eval',
    devServer: {
        contentBase: './app',
    },
};

module.exports = webpackConfig;
