// @flow strict

import { type Typography as TypographyTypes } from "./types";
import colors from "./colors";

const typography: TypographyTypes = {
  colors: {
    default: colors.color2,
  },
  fontFamily: {
    primary: "Roboto Condensed",
    secondary: "Anton",
  },
};

export default {
  colors,
  typography,
};
