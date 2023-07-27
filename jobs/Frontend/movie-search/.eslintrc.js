module.exports = {
  env: {
    browser: true, // Browser global variables like `window` etc.
    commonjs: true, // CommonJS global variables and CommonJS scoping.Allows require, exports and module.
    es6: true, // Enable all ECMAScript 6 features except for modules.
    jest: true, // Jest global variables like `it` etc.
    node: true, // Defines things like process.env when generating through node
  },
  extends: [
    "eslint:recommended",
    "plugin:react/recommended",
    "plugin:jsx-a11y/recommended",
    "plugin:react-hooks/recommended",
    "plugin:jest/recommended",
    "plugin:testing-library/react",
    "plugin:import/recommended" ,
    "plugin:import/typescript",
    "plugin:import/errors",
    "plugin:import/warnings",
  ],
  parser: "@babel/eslint-parser", // Uses babel-eslint transforms.
  parserOptions: {
    requireConfigFile: false,
    ecmaFeatures: {
      jsx: true,
    },
    ecmaVersion: 2018, // Allows for the parsing of modern ECMAScript features
    sourceType: "module", // Allows for the use of imports
  },
  plugins: [ "import" ], // eslint-plugin-import plugin. https://www.npmjs.com/package/eslint-plugin-import
  root: true, // For configuration cascading.
  rules: {
    "react/react-in-jsx-scope":"off"
  },
  
  settings: {
    react: {
      version: "detect", // Detect react version
    },
  },
  overrides: [
    {
        files: [ "**/*.ts?(x)" ],
        parser: "@typescript-eslint/parser",
        parserOptions: {
            ecmaFeatures: {
                jsx: true
            },
            ecmaVersion: 2018,
            sourceType: "module"
        },
        plugins: [
            "@typescript-eslint"
        ],
        // You can add Typescript specific rules here.
        // If you are adding the typescript variant of a rule which is there in the javascript
        // ruleset, disable the JS one.
        rules: {
            "@typescript-eslint/no-array-constructor": "warn",
            "no-array-constructor": "off",

        }
    }
],
};
