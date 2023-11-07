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

type Color = {
  [key in ColorBrightness]: string;
};

type ThemeColorVariants =
  | "main"
  | "on"
  | "container"
  | "onContainer"
  | "fixed"
  | "onFixed"
  | "surface"
  | "onSurface";

type ThemeColor = {
  [key in ThemeColorVariants]: string;
};

export interface ColorPalette {
  primary: Color;
  secondary: Color;
  tertiary: Color;
  error: Color;
  neutral: Color;
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
    error: {
      [key in ThemeColorVariants]?: string;
    };
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
