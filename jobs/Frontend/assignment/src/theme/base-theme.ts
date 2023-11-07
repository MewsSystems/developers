import { ColorPalette, GlobalTheme } from "./types";

interface BaseTheme extends Omit<GlobalTheme, "colors"> {
  colors: {
    palette: ColorPalette;
  };
}

export const colorPalette: ColorPalette = {
  primary: {
    "0": "#000000",
    "10": "#21005D",
    "20": "#381E72",
    "30": "#4F378B",
    "40": "#6750A4",
    "50": "#7F67BE",
    "60": "#9A82DB",
    "70": "#B69DF8",
    "80": "#D0BCFF",
    "90": "#EADDFF",
    "95": "#F6EDFF",
    "99": "#FFFBFE",
    "100": "#FFFFFF",
  },
  secondary: {
    "0": "#000000",
    "10": "#1D192B",
    "20": "#332D41",
    "30": "#4A4458",
    "40": "#625B71",
    "50": "#7A7289",
    "60": "#958DA5",
    "70": "#B0A7C0",
    "80": "#CCC2DC",
    "90": "#E8DEF8",
    "95": "#F6EDFF",
    "99": "#FFFBFE",
    "100": "#FFFFFF",
  },
  tertiary: {
    "0": "#000000",
    "10": "#31111D",
    "20": "#492532",
    "30": "#633B48",
    "40": "#7D5260",
    "50": "#986977",
    "60": "#B58392",
    "70": "#D29DAC",
    "80": "#EFB8C8",
    "90": "#FFD8E4",
    "95": "#FFECF1",
    "99": "#FFFBFA",
    "100": "#FFFFFF",
  },
  error: {
    "0": "#000000",
    "10": "#410E0B",
    "20": "#601410",
    "30": "#8C1D18",
    "40": "#B3261E",
    "50": "#DC362E",
    "60": "#E46962",
    "70": "#EC928E",
    "80": "#F2B8B5",
    "90": "#F9DEDC",
    "95": "#FCEEEE",
    "99": "#FFFBF9",
    "100": "#FFFFFF",
  },
  neutral: {
    "0": "#000000",
    "10": "#1D1B20",
    "20": "#322F35",
    "30": "#48464C",
    "40": "#605D64",
    "50": "#79767D",
    "60": "#938F96",
    "70": "#AEA9B1",
    "80": "#CAC5CD",
    "90": "#E6E0E9",
    "95": "#F5EFF7",
    "99": "#FFFBFF",
    "100": "#FFFFFF",
  },
};

const fonts = {
  displayLarge: {
    fontWeight: 300,
    fontSize: "6rem",
    lineHeight: "6rem",
  },
  displayMedium: {
    fontWeight: 300,
    fontSize: "3.75rem",
    lineHeight: "3.75rem",
  },
  displaySmall: {
    fontWeight: 300,
    fontSize: "3rem",
    lineHeight: "3rem",
  },
  headlineLarge: {
    fontWeight: 300,
    fontSize: "2.125rem",
    lineHeight: "2.125rem",
  },
  headlineMedium: {
    fontWeight: 300,
    fontSize: "1.5rem",
    lineHeight: "1.5rem",
  },
  headlineSmall: {
    fontWeight: 300,
    fontSize: "1.25rem",
    lineHeight: "1.25rem",
  },
  titleLarge: {
    fontWeight: 500,
    fontSize: "1.5rem",
    lineHeight: "1.5rem",
  },
  titleMedium: {
    fontWeight: 500,
    fontSize: "1.25rem",
    lineHeight: "1.25rem",
  },
  titleSmall: {
    fontWeight: 500,
    fontSize: "1rem",
    lineHeight: "1rem",
  },
  labelLarge: {
    fontWeight: 500,
    fontSize: "0.875rem",
    lineHeight: "0.875rem",
  },
  labelMedium: {
    fontWeight: 500,
    fontSize: "0.75rem",
    lineHeight: "0.75rem",
  },
  labelSmall: {
    fontWeight: 500,
    fontSize: "0.625rem",
    lineHeight: "0.625rem",
  },
  bodyLarge: {
    fontWeight: 400,
    fontSize: "1.25rem",
    lineHeight: "1.25rem",
  },
  bodyMedium: {
    fontWeight: 400,
    fontSize: "1rem",
    lineHeight: "1rem",
  },
  bodySmall: {
    fontWeight: 400,
    fontSize: "0.875rem",
    lineHeight: "0.875rem",
  },
};

export const baseTheme: BaseTheme = {
  fonts: fonts,
  colors: {
    palette: colorPalette,
  },
  layout: {
    breakpoints: {
      xs: "0px",
      sm: "600px",
      md: "905px",
      lg: "1240px",
      xl: "1440px",
    },
  },
};
