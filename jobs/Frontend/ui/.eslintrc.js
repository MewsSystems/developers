module.exports = {
  root: true,
  env: {
    node: true,
    es6: true,
  },
  parserOptions: { ecmaVersion: "latest", sourceType: "module" },
  ignorePatterns: ["node_modules/*"],
  extends: ["eslint:recommended", "next/core-web-vitals"],
  overrides: [
    {
      files: ["**/*.ts", "**/*.tsx"],
      parser: "@typescript-eslint/parser",
      settings: {
        react: { version: "detect" },
        "import/resolver": {
          typescript: {},
        },
      },
      env: {
        browser: true,
        node: true,
        es6: true,
      },
      extends: [
        "eslint:recommended",
        "plugin:import/errors",
        "plugin:import/warnings",
        "plugin:import/typescript",
        "plugin:@typescript-eslint/recommended",
        "plugin:react/recommended",
        "plugin:react-hooks/recommended",
        "plugin:jsx-a11y/recommended",
        "plugin:prettier/recommended",
        "plugin:tailwindcss/recommended",
        "plugin:vitest/legacy-recommended",
      ],
      rules: {
        "no-restricted-imports": [
          "error",
          {
            patterns: [
              {
                group: ["@/app/features/*/*"],
                message:
                  "Please import from the root @features folder (e.g. @features/ui instead of @features/ui/button).",
              },
            ],
          },
        ],
        "import/no-cycle": "off",
        "linebreak-style": ["error", "unix"],
        "react/prop-types": "off",
        "import/order": [
          "error",
          {
            groups: [
              "builtin",
              "external",
              "internal",
              "parent",
              "sibling",
              "index",
              "object",
            ],
            "newlines-between": "always",
            alphabetize: { order: "asc", caseInsensitive: true },
          },
        ],
        "no-empty-pattern": "off",
        "import/default": "off",
        "import/no-named-as-default-member": "off",
        "import/no-named-as-default": "off",
        "react/react-in-jsx-scope": "off",
        "jsx-a11y/anchor-is-valid": "off",
        "@typescript-eslint/no-unused-vars": ["error"],
        "@typescript-eslint/explicit-function-return-type": ["off"],
        "@typescript-eslint/explicit-module-boundary-types": ["off"],
        "@typescript-eslint/no-empty-function": ["off"],
        "@typescript-eslint/no-explicit-any": ["off"],
        "prettier/prettier": ["error", {}, { usePrettierrc: true }],
      },
    },
    {
      files: ["*.test.ts", "*.test.tsx"],
      extends: ["plugin:testing-library/react"],
    },
    {
      files: ["*.e2e.ts"],
      rules: {
        "playwright/expect-expect": "off",
      },
      extends: ["plugin:playwright/recommended"],
    },
  ],
}
