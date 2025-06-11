/*
 |-----------------------------------------------------------------------------
 | .lintstagedrc.js
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | See https://nextjs.org/docs/pages/building-your-application/configuring/eslint#lint-staged
 | for more information.
 */

module.exports = {
	'*.css': ['turbo lint:css --', 'prettier --write'],
	'*.{js,jsx,ts,tsx}': ['turbo lint:js --', 'prettier --write'],
};
