/*
 |-----------------------------------------------------------------------------
 | prettier.config.mjs
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | See https://prettier.io/docs/en/configuration.html for more information.
 |
 | @type {import('prettier').Config}
 */

const config = {
	arrowParens: 'always',
	printWidth: 80,
	semi: true,
	singleQuote: true,
	tabWidth: 4,
	trailingComma: 'all',
	useTabs: true,
	plugins: ['prettier-plugin-tailwindcss'],
};

export default config;
