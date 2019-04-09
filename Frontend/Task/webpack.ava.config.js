const path = require('path');

module.exports = {
  resolve: {
    modules: [
      __dirname,
      'node_modules',
      path.resolve(__dirname, 'src'),
    ],
    alias: {},
    extensions: ['.js', '.jsx', '.css'],
  },
  module: {
    rules: [
      {
        test: /\.(svg|js|jsx)$/,
        use: [{
          loader: 'babel-loader',
        }],
      },
    ],
  },
};
