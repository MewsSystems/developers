import "styled-components";

export interface Sizes {
  xs: string;
  sm: string;
  md: string;
  lg: string;
  xl: string;
}

export interface Colors {
  primary: string;
  primaryLight: string;
  secondary: string;
  tertiary: string;
  background: string;
  textPrimary: string;
  textSecondary: string;
  white: string;
  black: string;
}

export interface Typography {
  fontFamily: {
    primary: string;
    secondary: string;
  };
  fontSizes: Sizes;
  lineHeights: Sizes;
  headings: {
    fontFamily: string;
    fontWeight: number;
    sizes: {
      h1: {
        fontSize: string;
        lineHeight: string;
      };
      h2: {
        fontSize: string;
        lineHeight: string;
      };
      h3: {
        fontSize: string;
        lineHeight: string;
      };
      h4: {
        fontSize: string;
        lineHeight: string;
      };
      h5: {
        fontSize: string;
        lineHeight: string;
      };
      h6: {
        fontSize: string;
        lineHeight: string;
      };
    };
  };
}

interface Spacing extends Sizes {}

interface Breakpoint extends Sizes {}

declare module "styled-components" {
  export interface DefaultTheme {
    colors: Colors;
    typography: Typography;
    spacing: Spacing;
    breakpoint: Breakpoint;
    maxWidth: string;
  }
}
