/** @type {import('jest').Config} */
const config = {
  preset: 'ts-jest',
  testEnvironment: 'jsdom',
  moduleNameMapper: {
    '^@/(.*)$': '<rootDir>/src/$1',
  },
  setupFilesAfterEnv: ['<rootDir>/jest.setup.ts'],
  testMatch: [
    "**/src/**/_tests_/**/*.spec.[jt]s?(x)",
    "**/src/**/__tests__/**/*.spec.[jt]s?(x)"
  ],
  testPathIgnorePatterns: [
    "/node_modules/",
    "/tests/e2e/",
    "/_tests_/e2e/",
    "/__tests__/e2e/",
    "\\.e2e\\.spec\\.[jt]s?(x)$"
  ],
  transform: {
    '^.+\\.tsx?$': ['ts-jest', {
      tsconfig: 'tsconfig.test.json'
    }]
  },
  watchman: true,
  watchPlugins: [
    'jest-watch-typeahead/filename',
    'jest-watch-typeahead/testname'
  ],
  watchPathIgnorePatterns: [
    'node_modules',
    '.git',
    'dist',
    'coverage'
  ]
};

export default config; 