/** Stylelint configuration https://stylelint.io/user-guide/configure */

/** @type {import("stylelint").Config} */
module.exports = {
  rules: {
    /** only 6-digit or 8-digit formats are allowed (e.g. #RRGGBB or #RRGGBBAA) */
    "color-hex-length": "long",
    /** zero lengths do not need specified unit (e.g.{ margin: 0;}) */
    "length-zero-no-unit": true,
    /** there must be empty line before a new css at-rule (e.g. @media) */
    "at-rule-empty-line-before": "always",
    /** duplicate properties are not allowed (e.g. {color: white; color: pink;}) */
    "declaration-block-no-duplicate-properties": true,
    /** longhand properties can not be overwritten by shorthand properties (e.g. {padding-left: 10px; padding: 20px;}) */
    "declaration-block-no-shorthand-property-overrides": true,
    /** curly braces of css at-rule must not be empty (e.g. { }) */
    "block-no-empty": true,
    /** no vendor prefixes are needed (e.g. webkit, moz) */
    "value-no-vendor-prefix": true,
    /** no empty comments are allowed */
    "comment-no-empty": true
  }
}
