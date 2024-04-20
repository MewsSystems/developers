module.exports = {
  root: true,
  env: { browser: true, es2020: true },
  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:react-hooks/recommended",
  ],
  ignorePatterns: ["dist", ".eslintrc.cjs"],
  parser: "@typescript-eslint/parser",
  plugins: ["react-refresh", "simple-import-sort"],
  rules: {
    "react-refresh/only-export-components": [
      "warn",
      { allowConstantExport: true },
    ],
    "simple-import-sort/imports": "error",
    "simple-import-sort/exports": "error",
  },
  "overrides": [
    {
      "files": ["**/*.js", "**/*.ts", "**/*.tsx"],
      "rules": {
        "simple-import-sort/imports": [
          "error",
          {
            "groups": [
              // react, react-intl, react-router-dom, ...
              ["^react"],
              // package/third-party imports
              ["^@?\\w"],
              // mui libraries
              ["^@mui"],
              // project alias imports
              ["@api|@components|@hooks|@pages|@organisms|@pages|@styles"],
              // relative imports
              ["^[./]"]
            ]
          }
        ]
      }
    }
  ]
};
