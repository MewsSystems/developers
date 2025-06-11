/*
 |-----------------------------------------------------------------------------
 | tailwind.config.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 */

import type { Config } from 'tailwindcss';
import sharedConfig from '@repo/tailwind-config';

const config: Pick<Config, 'content' | 'presets'> = {
	content: ['./components/**/*.{ts,tsx}', './stories/**/*.mdx'],
	presets: [sharedConfig],
};

export default config;
