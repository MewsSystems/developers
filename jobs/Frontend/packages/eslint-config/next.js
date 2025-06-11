/*
 |-----------------------------------------------------------------------------
 | next.js
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | A custom ESLint configuration for libraries that use Next.js. See
 | https://eslint.org/docs/latest/use/configure/ for more information.
 |
 | @type {import('eslint').Linter.Config}
 */

import eslintConfigPrettier from 'eslint-config-prettier';
import globals from 'globals'
import js from '@eslint/js';
import pluginNext from '@next/eslint-plugin-next';
import pluginReact from 'eslint-plugin-react';
import pluginReactHooks from 'eslint-plugin-react-hooks';
import tseslint from 'typescript-eslint';

import { config as baseConfig } from './base.js';

const nextJsConfig = [
	...baseConfig,
	js.configs.recommended,
	eslintConfigPrettier,
	...tseslint.configs.recommended,
	{
		...pluginReact.configs.flat.recommended,
		languageOptions: {
			...pluginReact.configs.flat.recommended.languageOptions,
			globals: {
				...globals.serviceworker,
			},
		},
	},
	{
		plugins: {
			'@next/next': pluginNext,
		},
		rules: {
			...pluginNext.configs.recommended.rules,
			...pluginNext.configs['core-web-vitals'].rules,
		},
	},
	{
		plugins: {
			'react-hooks': pluginReactHooks,
		},
		settings: {
			react: {
				version: 'detect',
			},
		},
		rules: {
			...pluginReactHooks.configs.recommended.rules,
			// React scope no longer necessary with new JSX transform.
			"react/react-in-jsx-scope": "off",
		},
	},
];

export { nextJsConfig };
