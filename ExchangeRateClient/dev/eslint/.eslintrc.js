module.exports = {
	'parser': 'babel-eslint',
	'env': {
		'browser': true,
		'es6': true,
		'node': true
	},
	'extends': ['eslint:recommended', 'plugin:ramda/recommended', 'plugin:react/recommended'],
	'parserOptions': {
		'ecmaFeatures': {
			'experimentalObjectRestSpread': true,
			'blockBindings': true,
			'forOf': true,
			'jsx': true,
			'classes': true,
			'modules': true,
			'destructuring ': true
		},
		'sourceType': 'module'
	},
	'plugins': [
		'babel',
		'ramda',
		'react'
	],
    'settings': {
        'react': {
            'version': '16.0',
        },
    },
	'rules': {
		'arrow-body-style': 'error',
		'camelcase': 'error',
		'no-console': 'off',
		'no-extra-bind': 'error',
		'prefer-destructuring': ['error', {
			'array': true,
			'object': true
		}, {
			'enforceForRenamedProperties': false
		}],
		'prefer-rest-params': 'error',
		'prefer-spread': 'error',
		'no-mixed-spaces-and-tabs': 'off',
        
		'object-curly-spacing': [
			'error',
			'always'
		],
		'arrow-spacing': 'error',
		'block-spacing': [
			'error',
			'always'
		],
		'brace-style': 'error',
		'comma-dangle': [
			'error',
			'always-multiline'
		],
		'comma-spacing': [
			'error',
			{
				'before': false,
				'after': true
			}
		],
		'comma-style': [
			'error',
			'last'
		],
		'computed-property-spacing': [
			'error',
			'never'
		],
		'consistent-this': [
			'error',
			'self'
		],
		'consistent-return': 'off',
		'dot-notation': 'error',
		'dot-location': [
			'error',
			'property'
		],
		'eqeqeq': [
			'error',
			'smart'
		],
		'eol-last': 'error',
		'id-blacklist': [
			'error'
		],
		'keyword-spacing': 'error',
		'key-spacing': 'error',
		'max-len': [
			'error',
			130,
			4
		],
		'new-cap': [
			'off',
			{
				'capIsNew': true,
				'newIsCap': true
			}
		],
		'no-unused-expressions': [
			'error',
			{
				'allowShortCircuit': true,
				'allowTernary': true,
			}
		],
		'no-unused-vars': 'error',
		'no-shadow': 'off',
		'no-spaced-func': 'error',
		'no-multiple-empty-lines': 'error',
		'no-multi-spaces': 'error',
		'no-undef': 'error',
		'no-empty-pattern': 'error',
		'no-dupe-keys': 'error',
		'no-dupe-args': 'error',
		'no-duplicate-case': 'error',
		'no-cond-assign': 'error',
		'no-extra-semi': 'error',
		'no-extra-boolean-cast': 'error',
		'no-trailing-spaces': 'error',
		'no-unneeded-ternary': 'error',
		'no-unreachable': 'error',
		'no-var': 'error',
		'one-var': [
			'error',
			'never'
		],
		'operator-linebreak': [
			'error',
			'after'
		],
		'prefer-arrow-callback': 'off',
		'prefer-const': 'error',
		'prefer-template': 'error',
		'quotes': [
			'error',
			'single',
			'avoid-escape'
		],
		'semi': [
			'error',
			'always'
		],
		'space-before-blocks': [
			'error',
			'always'
		],
		'space-before-function-paren': [
			'error',
			'never'
		],
		'space-infix-ops': 'error',
		'space-unary-ops': [
			'error',
			{
				'words': true,
				'nonwords': false
			}
		],
		'spaced-comment': 'error',
		'yoda': 'error',
		'strict': 'off',
		'no-case-declarations': 'off',
		'generator-star-spacing': 'error',
		'array-bracket-spacing': 'error',
		'arrow-parens': 'error',
		'no-await-in-loop': 'error',
		'babel/new-cap': 'error',
		'object-shorthand': 'error',
		'jsx-quotes': [
			'error',
			'prefer-double'
		],
		'react/display-name': 'error',
		'react/jsx-boolean-value': [
			'error',
			'never'
		],
		'react/jsx-closing-bracket-location': 'error',
		'react/jsx-curly-spacing': 'error',
		'react/jsx-equals-spacing': 'error',
		'react/jsx-filename-extension': [
			'error',
			{
				'extensions': [
					'.js'
				]
			}
		],
		'react/jsx-first-prop-new-line': 'off',
		'react/jsx-handler-names': [
			'error',
			{
				'eventHandlerPrefix': '_?handle'
			}
		],
		'react/jsx-indent-props': 'off',
		'react/jsx-max-props-per-line': 'off',
		'react/jsx-no-duplicate-props': 'error',
		'react/jsx-no-undef': 'error',
		'react/jsx-pascal-case': 'error',
		'react/jsx-tag-spacing': 'error',
		'react/jsx-uses-react': 'error',
		'react/jsx-uses-vars': 'error',
		'react/jsx-no-comment-textnodes': 'error',
		'react/no-danger': 'error',
		'react/no-deprecated': 'error',
		'react/no-did-mount-set-state': 'error',
		'react/no-did-update-set-state': 'error',
		'react/no-direct-mutation-state': 'error',
		'react/no-multi-comp': 'off',
		'react/no-render-return-value': 'error',
		'react/no-is-mounted': 'error',
		'react/no-unknown-property': 'error',
		'react/prefer-arrow-callback': 'off',
		'react/prefer-es6-class': 'error',
		'react/prop-types': 'error',
		'react/react-in-jsx-scope': 'error',
		'react/require-render-return': 'error',
		'react/self-closing-comp': 'error',
		'react/sort-comp': 'error',
		'react/sort-prop-types': 'error',
		'react/no-string-refs': 'warn',
		'react/jsx-key': 'error',
		'react/jsx-no-bind': 'off',
		'react/jsx-no-literals': 'off',
		'react/jsx-no-target-blank': 'off',
		'react/jsx-sort-props': 'off',
		'react/no-set-state': 'off',
		'react/forbid-prop-types': 'off',
		'react/prefer-stateless-function': 'off',
		'react/require-optimization': 'off'
	}
};