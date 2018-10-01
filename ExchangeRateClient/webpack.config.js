const webpack = require('webpack');

const webpackConfig = {
    plugins: [
        new webpack.NoErrorsPlugin(),
    ],
    entry: {
        app: './app/app.js',
    },
    output: {
        filename: 'app.js',
        library: 'app',
        libraryTarget: 'window',
        publicPath: '/',
    },   

    resolve: {
        extensions: ['', '.js', '.json'],
    },
    module: {
        loaders: [{
            test: /\.js?$/,
            exclude: /(node_modules|Generated)/,
            loader: 'babel',
        }, {
            test: /\.json$/,
            loader: 'json',
        },
        {test: /(\.css)$/,
       loaders: ['style-loader', 'css-loader']
    }
     
    ],
    },
    devtool: 'eval',
    devServer: {
        contentBase: './app',
    },
};

module.exports = webpackConfig;
