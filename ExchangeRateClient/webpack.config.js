const webpack = require("webpack");
const path = require("path");

const mode =
  process.env.NODE_ENV === "production" ? "production" : "development";

const webpackConfig = {
  mode,
  plugins: [new webpack.NoEmitOnErrorsPlugin()],
  entry: {
    app: "./app/index.js"
  },
  output: {
    filename: "main.js",
    path: path.resolve(__dirname, "./app/dist")
  },
  resolve: {
    extensions: [".js", ".json"]
  },
  module: {
    rules: [
      {
        test: /\.js?$/,
        exclude: /(node_modules|Generated)/,
        loader: "babel-loader"
      },
      {
        test: /\.json$/,
        loader: "json"
      },
      {
        test: /\.svg$/,
        use: [
          {
            loader: "babel-loader"
          },
          {
            loader: "react-svg-loader",
            options: {
              jsx: true // true outputs JSX tags
            }
          }
        ]
      }
    ]
  },
  devtool: "eval",
  devServer: {
    contentBase: "./app/dist"
  }
};

module.exports = webpackConfig;
