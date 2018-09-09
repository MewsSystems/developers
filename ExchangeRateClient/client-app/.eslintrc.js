module.exports = {
  parser: "babel-eslint",
  env: {
    "jest/globals": true,
  },
  extends: [
    "airbnb",
    "prettier",
    "prettier/flowtype",
    "prettier/react",
    "plugin:jest/recommended",
    "plugin:flowtype/recommended",
  ],
  plugins: ["prettier", "jest", "flowtype"],
  rules: {
    "prettier/prettier": "error",
  },
};
