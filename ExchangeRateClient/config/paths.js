const path = require('path');
const fs = require('fs');

const appDirectory = fs.realpathSync(process.cwd());
const resolveApp = relativePath => path.resolve(appDirectory, relativePath);

module.exports = {
  appHtml: resolveApp('app/index.html'),
  appIndexJs: resolveApp('app/index.jsx'),
  appSrc: resolveApp('app'),
  appConfig: resolveApp('config'),
};
