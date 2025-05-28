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
        'plugin:prettier/recommended', // enables prettier integration
    ],
    rules: {
        'prettier/prettier': ['error'],
        // optional:
        'react/react-in-jsx-scope': 'off', // for React 17+
        'simple-import-sort/imports': 'error',
        'simple-import-sort/exports': 'error',
    },
};