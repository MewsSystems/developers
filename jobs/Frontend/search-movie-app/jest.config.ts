export default {
  rootDir: 'src',
  preset: 'ts-jest',
  testEnvironment: 'jsdom',
  transform: {
    '^.+\\.tsx?$': [
      'ts-jest',
      {
        diagnostics: {
          ignoreCodes: [1343],
        },
        astTransformers: {
          before: [
            {
              path: 'node_modules/ts-jest-mock-import-meta',
              options: {
                metaObjectReplacement: {
                  env: {
                    VITE_API_KEY:
                      'eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI0MDI4YmU0M2ViYjhmZGNjMzc2YjhmNzA5MDlmNTU2NSIsIm5iZiI6MTc0OTkwNjYzOS45ODQsInN1YiI6IjY4NGQ3NGNmM2E4YjIyOWM1N2JiNDllMCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.alxqte0ZNvDX2NYsh27jCuXMQczd48nYOcbrjFDuAVw',
                  },
                },
              },
            },
          ],
        },
      },
    ],
  },
  setupFilesAfterEnv: ['../jest.setup.ts'],
  moduleFileExtensions: [
    'tsx',
    'ts',
    'web.js',
    'js',
    'web.ts',
    'web.tsx',
    'json',
    'web.jsx',
    'jsx',
    'node',
  ],
  modulePaths: ['<rootDir>/src'],
};
