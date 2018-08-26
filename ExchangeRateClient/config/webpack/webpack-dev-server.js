const WebpackDevServer = require('webpack-dev-server');
const webpack = require('webpack');
const paths = require('../paths');

const config = require('./webpack-dev-config.js');

const options = {
  open: true,
  contentBase: paths.appBuild,
  hot: true,
  host: 'localhost',
  quiet: false,
  historyApiFallback: true,
  overlay: {
    warnings: false,
    errors: true,
  },
  after() {
    // eslint-disable-next-line no-console
    console.log('dev server running http://localhost:8080');
  },
};

WebpackDevServer.addDevServerEntrypoints(config, options);
const compiler = webpack(config);
const server = new WebpackDevServer(compiler, options);

server.listen(8080, 'localhost', () => {});
