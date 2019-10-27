/* eslint-disable @typescript-eslint/no-var-requires */
const process = require('process');
const packageJson = require('./package.json');

const nonExactVersionPattern = /^[^0-9]+/;

const errors = Object.entries(packageJson.dependencies)
  .map(([packageName, version]) => {
    const matches = version.match(nonExactVersionPattern);
    if (matches) {
      return `${packageName} has non-exact version ${version} (please remove ${
        matches[0]
      })`;
    }
  })
  .filter(Boolean);

if (packageJson.devDependencies) {
  errors.push('Please move devDependencies to dependencies');
}

if (errors.length > 0) {
  console.log('\nThere are issues with package.json:');
  console.log(errors.map(error => `  - ${error}`).join('\n'));
  console.log('\n');
  process.exit(1);
}
