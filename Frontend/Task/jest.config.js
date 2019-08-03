// @flow

// For a detailed explanation regarding each configuration property, visit:
// https://jestjs.io/docs/en/configuration.html

module.exports = {
  clearMocks: true,
  coverageDirectory: 'coverage',
  setupFiles: ['./app/jestSetup/config.js'],
    setupFilesAfterEnv: ['./app/jestSetup/setupTests.js'],
};
