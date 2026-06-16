module.exports = {
  testEnvironment: "jsdom",
  testEnvironmentOptions: {
    customExportConditions: [""],
  },
  setupFiles: [
    "<rootDir>/src/mocks/jest/jest.polyfills.ts"
  ],
  setupFilesAfterEnv: [
    "<rootDir>/jest-setup.js"
  ],
  testPathIgnorePatterns: [
    "<rootDir>/node_modules",
    "<rootDir>/dist"
  ],
  transform: {
    "\\.[jt]sx?$": "babel-jest",
    "^.+\\.svg$": "<rootDir>/src/mocks/jest/svgTransform.ts"
  }
};
