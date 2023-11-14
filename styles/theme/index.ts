import { Glass_Antiqua, Montserrat } from "next/font/google";
import { DefaultTheme } from "styled-components/dist/types";
import { rem } from "@/util/styles";

const primaryFont = Montserrat({
  subsets: ["latin"],
  weight: ["200", "300", "400", "500", "600", "700", "800"],
});
const secondaryFont = Glass_Antiqua({ subsets: ["latin"], weight: ["400"] });

const baseFontSize = 16;
const baseHeadingFontWeight = 700;

export const theme: DefaultTheme = {
  maxWidth: "1440px",
  colors: {
    primary: "#1A76F8",
    primaryLight: "#98C2FC",
    secondary: "#FB1A1D",
    tertiary: "#E9E9E9",
    background: "#010102",
    textPrimary: "#F7F7F7",
    textSecondary: "#D4D4D4",
    white: "#FFFFFF",
    black: "#000000",
  },
  typography: {
    fontFamily: {
      primary: primaryFont.style.fontFamily,
      secondary: secondaryFont.style.fontFamily,
    },
    fontSizes: {
      xs: rem(12),
      sm: rem(14),
      md: rem(baseFontSize),
      lg: rem(20),
      xl: rem(24),
    },
    lineHeights: {
      xs: rem(16),
      sm: rem(18),
      md: rem(22),
      lg: rem(28),
      xl: rem(32),
    },
    headings: {
      fontFamily: primaryFont.style.fontFamily,
      fontWeight: baseHeadingFontWeight,
      sizes: {
        h1: {
          fontSize: rem(36),
          lineHeight: rem(42),
        },
        h2: {
          fontSize: rem(32),
          lineHeight: rem(36),
        },
        h3: {
          fontSize: rem(24),
          lineHeight: rem(28),
        },
        h4: {
          fontSize: rem(20),
          lineHeight: rem(22),
        },
        h5: {
          fontSize: rem(18),
          lineHeight: rem(20),
        },
        h6: {
          fontSize: rem(16),
          lineHeight: rem(18),
        },
      },
    },
  },
  spacing: {
    xs: rem(8),
    sm: rem(12),
    md: rem(16),
    lg: rem(24),
    xl: rem(32),
  },
  breakpoint: {
    xs: "36em",
    sm: "48em",
    md: "62em",
    lg: "75em",
    xl: "88em",
  },
};
