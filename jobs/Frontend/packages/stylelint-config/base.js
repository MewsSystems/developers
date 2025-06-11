/*
 |-----------------------------------------------------------------------------
 | base.js
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | A shared StyleLint configuration for the repository. See
 | https://stylelint.io/user-guide/configure/ for more information.
 |
 | @type {import('stylelint').Config}
 */

const config = {
	extends: ['stylelint-config-standard'],
	plugins: ['stylelint-order'],
	rules: {
		'at-rule-no-deprecated': [
			true,
			{
				ignoreAtRules: ['apply'],
			},
		],
		'at-rule-no-unknown': [
			true,
			{
				ignoreAtRules: ['custom-variant', 'theme', 'slot', 'utility'],
			},
		],
		'color-hex-length': 'long',
		'custom-property-pattern': null,
		'function-no-unknown': [
			true,
			{
				ignoreFunctions: ['theme'],
			},
		],
		'import-notation': 'string',
	},
};

export { config };
