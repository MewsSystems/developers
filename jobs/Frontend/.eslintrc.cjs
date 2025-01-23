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
  plugins: ["react-refresh"],
  rules: {
    "@typescript-eslint/no-explicit-any": "off",
    "no-case-declarations": "off",
    "no-undef": "off",
    "react-hooks/exhaustive-deps": "off",
  },
  overrides: [
    {
      files: ["**/*.{js,jsx,ts,tsx}"],
      env: {
        jest: true,
      },
    },
  ],
}
