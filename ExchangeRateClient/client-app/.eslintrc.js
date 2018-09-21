module.exports = {
  parser: "babel-eslint",
  env: {
    "jest/globals": true,
    browser: true,
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
    "react/jsx-filename-extension": [1, { extensions: [".js", ".jsx"] }],
    "react/destructuring-assignment": [0],
  },
};
