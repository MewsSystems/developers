const path = require("path");

module.exports = {
  // plugins: [new webpack.NoEmitOnErrorsPlugin()],
  entry: {
    app: "./app/src/index.js"
  },
  output: {
    path: path.resolve(__dirname, "build"),
    filename: "bundle.js"
  },
  resolve: {
    extensions: [".js", ".json"]
  },
  module: {
    rules: [
      {
        test: /\.js?$/,
        exclude: /node_modules/,
        loader: "babel-loader"
      },
      {
        test: /\.json$/,
        loader: "json-loader"
      }
    ]
  },
  devtool: "eval",
  devServer: {
    contentBase: "./app"
  }
};
