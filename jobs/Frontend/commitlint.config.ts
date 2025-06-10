/*
 |-----------------------------------------------------------------------------
 | commitlint.config.ts
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | See https://commitlint.js.org/reference/configuration.html for more
 | information.
 |
 | Type description:
 |
 | build:    Changes that affect external dependencies
 | ci:       Changes to CI configuration files and scripts
 | docs:     Changes that affect only documentation
 | feat:     Changes that introduce a new feature
 | fix:      Changes that fix a bug
 | perf:     Changes that improve performance
 | refactor: Changes that neither fixes a bug nor adds a feature
 | style:    Changes that do not affect the meaning of the code, e.g. white-space,
 |           formatting, missing semicolons, etc
 | test:     Changes that add missing tests or correct existing tests
 */

import { RuleConfigSeverity, type UserConfig } from '@commitlint/types';

const types: string[] = [
	'build',
	'ci',
	'docs',
	'feat',
	'fix',
	'perf',
	'refactor',
	'style',
	'test',
];

const Configuration: UserConfig = {
	extends: ['@commitlint/config-conventional'],
	rules: {
		'header-max-length': [RuleConfigSeverity.Error, 'always', 72],
		'subject-case': [RuleConfigSeverity.Error, 'always', 'lower-case'],
		'type-enum': [RuleConfigSeverity.Error, 'always', types],
	},
};

export default Configuration;
