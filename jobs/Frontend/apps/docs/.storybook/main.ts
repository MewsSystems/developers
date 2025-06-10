/*
 |-----------------------------------------------------------------------------
 | .storybook/main.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | Some description...
 */

import { dirname, join } from 'path';

import type { StorybookConfig } from '@storybook/nextjs';

const getAbsolutePath = (value) => {
	return dirname(require.resolve(join(value, 'package.json')));
};

const config: StorybookConfig = {
	addons: [
		getAbsolutePath('@storybook/addon-essentials'),
		getAbsolutePath('@storybook/addon-interactions'),
		getAbsolutePath('@storybook/addon-a11y'),
		getAbsolutePath('storybook-addon-pseudo-states'),
		getAbsolutePath('@etchteam/storybook-addon-status'),
		getAbsolutePath('storybook-mobile-addon'),
		getAbsolutePath('@storybook/addon-links'),
		getAbsolutePath('@storybook/addon-themes'),
	],

	features: {
		experimentalRSC: true,
	},

	framework: {
		name: getAbsolutePath('@storybook/nextjs'),
		options: {},
	},

	staticDirs: ['../public'],

	stories: [
		'../stories/**/*.mdx',
		'../../../packages/ui/src/components/**/*.stories.@(js|jsx|ts|tsx)',
	],
};

export default config;
