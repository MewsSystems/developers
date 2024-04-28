/** @type {import("prettier").Config} */
module.exports = {
  /** Include parentheses around a sole arrow function parameter. */
  arrowParens: "always",
  /** Print spaces between brackets in object literals. */
  bracketSpacing: true,
  /** For historical reasons, there exist two common flavors of line endings in text files. That is \n (or LF for Line Feed) and \r\n (or CRLF for Carriage Return + Line Feed). The former is common on Linux and macOS, while the latter is prevalent on Windows. */
  endOfLine: "lf",
  /** Print semicolons at the ends of statements. */
  semi: false,
  /** Use single quotes instead of double quotes. */
  singleQuote: false,
  /** Indent lines with tabs instead of spaces. */
  useTabs: false,
  /** Specify the number of spaces per indentation-level. */
  tabWidth: 2,
  /** Print trailing commas wherever possible in multi-line comma-separated syntactic structures. (A single-line array, for example, never gets trailing commas.) */
  trailingComma: "es5",
  /** Specify the line length that the printer will wrap on. */
  printWidth: 80,
}
