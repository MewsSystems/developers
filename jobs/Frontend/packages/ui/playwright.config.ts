/*
 |-----------------------------------------------------------------------------
 | playwright.config.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | See https://playwright.dev/docs/test-configuration for more information.
 */

import { resolve } from 'path';

import { config } from '@repo/testing/components/config';
import { defineConfig } from '@repo/testing/components';
import dotenv from 'dotenv';

dotenv.config({ path: '.env' });

export default defineConfig({
	...config,

	testDir: './src/components',

	outputDir: './playwright/results',

	reporter: process.env.CI
		? 'list'
		: [['html', { outputFolder: './playwright/report' }]],

	use: {
		trace: 'on-first-retry',

		ctPort: 3100,

		ctViteConfig: {
			resolve: {
				alias: {
					'@ui': resolve(__dirname, './src'),
				},
			},
		},
	},
});
