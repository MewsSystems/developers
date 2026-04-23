module.exports = {
  extends: ['eslint:recommended', 'plugin:react/recommended'],
  parser: 'babel-eslint',
  plugins: ['react', 'prettier', 'import'],
  rules: {
    'prettier/prettier': 'error',
    'import/order': [
      'warn',
      {
        groups: ['builtin', 'external', 'parent', 'sibling', 'index'],
        pathGroups: [
          {
            pattern: 'react',
            group: 'builtin',
            position: 'before',
          },
        ],
        'newlines-between': 'always',
        alphabetize: {
          order: 'asc',
          caseInsensitive: true,
        },
      },
    ],
  },

  settings: {
    react: {
      version: 'detect',
    },
  },
};
