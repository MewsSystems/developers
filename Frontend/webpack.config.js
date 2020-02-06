var debug = process.env.NODE_ENV !== "production";
var path = require('path');
const TerserPlugin = require('terser-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const HtmlWebpackPlugin = require('html-webpack-plugin');

module.exports = {
  //context: path(__dirname,), 
  devtool: debug ? "inline-sourcemap" : null,
  entry: "./src/scripts.tsx",
  mode: 'production',
  plugins:  [
     new HtmlWebpackPlugin({
      template: './src/index.html',
      inject: true,
    }),
    new MiniCssExtractPlugin({filename: "assets/style.min.css", }),   
      ],
  optimization: {
    minimize: true,
    minimizer: [
      new TerserPlugin({
        test: /\.ts(\?.*)?$/i,
      }),
    ],
  },
  resolve: {
    extensions: [".js", ".json", ".ts", ".tsx"],
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        exclude: /(node_modules)/,
        loader: 'babel-loader',  
       
      },
      {
        test: /\.sass$/,
        use: [
         MiniCssExtractPlugin.loader,
         "css-loader",
         "sass-loader"
        ]
      },
      {
        test: /\.(png|jpe?g|gif|png)$/,
        use: [
          {
            loader: 'file-loader',
            options: {
              outputPath: 'assets',
              name: '[name].[ext]',
            },
          },
        ],
      },    
    ]
  },
  output: {
    path: __dirname + '/build',
    filename: "scripts.min.js",
    publicPath: '/',
     },
  devServer: {
    historyApiFallback: true,
    contentBase: path.join(__dirname, 'src'),
    watchContentBase: true,
    compress: true,
    host: 'localhost',
    port: 8000,
    open: true,
  },
    
};
