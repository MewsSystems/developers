/*
 |-----------------------------------------------------------------------------
 | tailwind.config.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Config } from 'tailwindcss';
import sharedConfig from '@repo/tailwind-config';

const config: Pick<Config, 'content' | 'presets'> = {
	content: ['./src/**/*.{ts,tsx}'],
	presets: [sharedConfig],
};

export default config;
