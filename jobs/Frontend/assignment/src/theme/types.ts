// Color types
type ColorBrightness =
  | "0"
  | "10"
  | "20"
  | "30"
  | "40"
  | "50"
  | "60"
  | "70"
  | "80"
  | "90"
  | "95"
  | "99"
  | "100";

type ExtendedColorBrightness =
  | "4"
  | "6"
  | "12"
  | "17"
  | "22"
  | "24"
  | "87"
  | "92"
  | "94"
  | "96"
  | "98";

type Color = {
  [key in ColorBrightness]: string;
};

type ExtendedColor = Color & {
  [key in ExtendedColorBrightness]: string;
};

type ThemeColorVariants =
  | "main"
  | "on"
  | "container"
  | "onContainer"
  | "fixed"
  | "fixedDim"
  | "onFixed"
  | "onFixedVariant";

type ThemeColor = {
  [key in ThemeColorVariants]: string;
};

export interface ColorPalette {
  primary: Color;
  secondary: Color;
  tertiary: Color;
  error: Color;
  neutral: ExtendedColor;
  neutralVariant: Color;
}

// Font types
export type TypographyVariant =
  | "displayLarge"
  | "displayMedium"
  | "displaySmall"
  | "headlineLarge"
  | "headlineMedium"
  | "headlineSmall"
  | "titleLarge"
  | "titleMedium"
  | "titleSmall"
  | "labelLarge"
  | "labelMedium"
  | "labelSmall"
  | "bodyLarge"
  | "bodyMedium"
  | "bodySmall";

export type Font = {
  fontWeight: number;
  fontSize: string;
  lineHeight: string;
};

// Layout types
export type Breakpoint = "xs" | "sm" | "md" | "lg" | "xl";

// Theme types
export interface GlobalTheme {
  colors: {
    palette: ColorPalette;
    primary: ThemeColor;
    secondary: ThemeColor;
    tertiary: ThemeColor;
    error: Pick<ThemeColor, "main" | "on" | "container" | "onContainer">;
    surface: Pick<ThemeColor, "main" | "on"> & {
      variant: string;
      onVariant: string;
      inverse: string;
      inverseOn: string;
      tint: string;
      containerHighest: string;
      containerHigh: string;
      container: string;
      containerLow: string;
      containerLowest: string;
      bright: string;
      dim: string;
    };
    outline: Pick<ThemeColor, "main"> & { variant: string };
  };
  fonts: {
    [key in TypographyVariant]: Font;
  };
  layout: {
    breakpoints: {
      [key in Breakpoint]: string;
    };
  };
}
