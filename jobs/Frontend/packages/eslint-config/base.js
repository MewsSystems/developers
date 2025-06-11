/*
 |-----------------------------------------------------------------------------
 | base.js
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | A shared ESLint configuration for the repository. See
 | https://eslint.org/docs/latest/use/configure/ for more information.
 |
 | @type {import('eslint').Linter.Config}
 */

import eslintConfigPrettier from 'eslint-config-prettier';
import js from '@eslint/js';
import onlyWarn from 'eslint-plugin-only-warn';
import pluginImport from 'eslint-plugin-import';
import pluginJsxA11y from 'eslint-plugin-jsx-a11y';
import tseslint from 'typescript-eslint';
import turboPlugin from 'eslint-plugin-turbo';

const config = [
	js.configs.recommended,
	eslintConfigPrettier,
	{
		plugins: {
			tseslint: tseslint,
		},
		rules: {
			'@typescript-eslint/consistent-type-imports': 'warn',
		},
	},
	{
		plugins: {
			turbo: turboPlugin,
		},
		rules: {
			'turbo/no-undeclared-env-vars': 'warn',
		},
	},
	{
		plugins: {
			import: pluginImport,
		},
		rules: {
			'import/no-anonymous-default-export': [
				'error',
				{
					allowArray: false,
					allowArrowFunction: false,
					allowAnonymousClass: false,
					allowAnonymousFunction: false,
					allowCallExpression: true, // The true value here is for backward compatibility
					allowLiteral: false,
					allowObject: true,
				},
			],
			'import/order': [
				'error',
				{
					groups: ['builtin', 'external', 'internal'],
					pathGroups: [
						{
							pattern: 'react',
							group: 'external',
							position: 'before',
						},
					],
					pathGroupsExcludedImportTypes: ['react'],
					'newlines-between': 'always',
				},
			],
			'sort-imports': [
				'error',
				{
					ignoreCase: false,
					ignoreDeclarationSort: false,
					ignoreMemberSort: false,
					memberSyntaxSortOrder: [
						'none',
						'all',
						'multiple',
						'single',
					],
					allowSeparatedGroups: true,
				},
			],
		},
	},
	{
		plugins: {
			pluginJsxA11y,
		},
	},
	{
		plugins: {
			onlyWarn,
		},
	},
	{
		ignores: ['dist/**', 'lib/testing', 'public', 'src/lib/testing'],
	},
];

export { config };
