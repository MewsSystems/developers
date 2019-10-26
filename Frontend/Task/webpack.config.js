const webpackConfig = {
  optimization: {
    noEmitOnErrors: true,
  },
  entry: {
    app: './app/app.ts',
  },
  output: {
    filename: '[name].js',
    library: 'app',
    libraryTarget: 'window',
  },
  resolve: {
    extensions: ['.js', '.json', '.ts', '.tsx'],
  },
  module: {
    rules: [
      {
        test: /\.[jt]sx?$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
    ],
  },
  devtool: 'eval',
  devServer: {
    contentBase: './app',
  },
};

module.exports = webpackConfig;
