const path = require('path');
const webpack = require('webpack');


const PATHS = {
    app: path.join(__dirname, 'app'),
    build: path.join(__dirname, 'app/dist'),
};

const rules = {
    js: {
        test: /\.(js|jsx)$/,
        include: [PATHS.app],
        use: {
          loader: 'babel-loader',
          options: {
            presets: ['react']
          }
        }
    },
    less: {
        test: /\.less$/,
        include: [PATHS.app],
        use: [
          { loader: "style-loader" },
          {
            loader: "css-loader",
            options: {
               modules: true,
               camelCase: true,
               importLoaders: 1,
               localIdentName: '[name]_[local]_[hash:base64:5]'
           }
          },
          {
            loader: "less-loader",
            options: {
                strictMath: true,
                noIeCompat: true
            }
          }
        ],
    }
};

const config = {
    entry: {
      app: [
          'whatwg-fetch',
          './app/src/index.js',
      ]
    },
    output: {
        path: PATHS.build,
        filename: '[name].js',
        publicPath: '/',
    },
    target: 'web',
    resolve: {
        extensions: ['.js', '.jsx', '.json', '.less'],
        modules: [PATHS.app, 'node_modules'],
        enforceExtension: false,
    },
    module: {
        rules: [
            rules.js,
            rules.less,
        ],
    },
    plugins: [
        new webpack.NoEmitOnErrorsPlugin(),
    ],
    devtool: 'source-map',
    devServer: {
        contentBase: path.join(__dirname, '/app/dist'),
        staticOptions: {
            index: 'index.html',
        },
        open: true,
        proxy: {
            '/api': {
                target: 'http://localhost:3000/',
                pathRewrite: {'^/api' : ''}
            },
        },
        historyApiFallback: {
            rewrites: [{
                from: /^\/((?!api|assets).)*$/,
                to: '/',
            }],
        },
        hot: true,
        watchOptions: {
            aggregateTimeout: 300,
            poll: 1000,
        },
    },
};

module.exports = config;
