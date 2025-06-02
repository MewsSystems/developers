import {defineConfig, devices} from '@playwright/test';

export default defineConfig({
  testDir: './src/app/_tests_/e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: 3,
  workers: 3,
  reporter: [
    [
      'html',
      {
        open: 'never',
        outputFolder: './src/app/_tests_/e2e/playwright-report',
      },
    ],
  ],
  outputDir: './src/app/_tests_/e2e/test-results',
  timeout: 45000,
  use: {
    baseURL: 'http://localhost:5173',
    trace: 'retain-on-failure',
    screenshot: 'only-on-failure',
  },
  projects: [
    {
      name: 'chromium',
      use: {...devices['Desktop Chrome']},
    },
  ],
  webServer: {
    command: 'yarn start:app',
    url: 'http://localhost:5173',
    reuseExistingServer: !process.env.CI,
    timeout: 120000,
  },
});
