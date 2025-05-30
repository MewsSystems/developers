module.exports = {
    root: true,
    env: { browser: true, es2021: true },
    parser: '@typescript-eslint/parser',
    parserOptions: { ecmaVersion: 'latest', sourceType: 'module' },
    plugins: ['react', '@typescript-eslint', 'prettier', 'simple-import-sort'],
    extends: [
        'eslint:recommended',
        'plugin:react/recommended',
        'plugin:@typescript-eslint/recommended',
        'plugin:prettier/recommended',
    ],
    rules: {
        'prettier/prettier': ['error'],
        'react/react-in-jsx-scope': 'off',
        'simple-import-sort/imports': 'error',
        'simple-import-sort/exports': 'error',
        '@typescript-eslint/no-require-imports': 'error',
        '@typescript-eslint/consistent-type-imports': ['error', {
            prefer: 'type-imports',
            disallowTypeAnnotations: false,
        }],
    },
    settings: {
        react: {
            version: 'detect'
        }
    }
};