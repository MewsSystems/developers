const webpack = require('webpack');

const webpackConfig = {
    plugins: [
        new webpack.NoErrorsPlugin(),
    ],
    entry: {
        app: './app/app.js',
    },
    output: {
        filename: '[name].js',
        library: 'app',
        libraryTarget: 'window',
    },
    resolve: {
        extensions: ['', '.js', '.json'],
    },
    presets: ["@babel/preset-env", "@babel/preset-react"],
    module: {
        loaders: [{
             test: /\.(js|jsx)$/,
            exclude: /(node_modules|Generated)/,
            loader: 'babel-loader',
            query: {
                presets: ['react']
            }
        }, {
            test: /\.json$/,
            loader: 'json',
        }, {
          test: /\.(scss|css)$/,
          loader: 'style!css-loader?modules&importLoaders=1&localIdentName=[name]__[local]___[hash:base64:5]'
        }, {
            test: /\.scss$/,
            loader: "sass-loader",
        }, {
            test: /\.(woff(2)?|ttf|eot|svg)(\?v=\d+\.\d+\.\d+)?$/,
            loader: 'file-loader',
            exclude: /images/,
            query: {
                name: '[name].[ext]',
                outputPath: 'fonts/'
            }
        }
        ],
    },
    devtool: 'eval',
    devServer: {
        contentBase: './app',
    },
};

module.exports = webpackConfig;
