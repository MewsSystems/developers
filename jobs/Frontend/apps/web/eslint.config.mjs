/*
 |-----------------------------------------------------------------------------
 | eslint.config.mjs
 | v1.0.0
 |-----------------------------------------------------------------------------
 |
 | See packages/eslint-config for more information.
 |
 | @type {import('postcss-load-config').Config}
 */

import { nextJsConfig } from '@repo/eslint-config/next-js';

/** @type {import('eslint').Linter.Config} */
export default nextJsConfig;
