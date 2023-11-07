import { DefaultTheme } from "styled-components";
import { baseTheme, colorPalette } from "./base-theme";

export const materialLightTheme: DefaultTheme = {
  ...baseTheme,
  colors: {
    ...baseTheme.colors,
    primary: {
      main: colorPalette.primary[40],
      on: colorPalette.primary[100],
      container: colorPalette.primary[90],
      onContainer: colorPalette.primary[10],
      fixed: colorPalette.primary[90],
      onFixed: colorPalette.primary[10],
      surface: colorPalette.primary[100],
      onSurface: colorPalette.primary[100],
    },
    secondary: {
      main: colorPalette.secondary[40],
      on: colorPalette.secondary[100],
      container: colorPalette.secondary[90],
      onContainer: colorPalette.secondary[10],
      fixed: colorPalette.secondary[90],
      onFixed: colorPalette.secondary[10],
      surface: colorPalette.secondary[100],
      onSurface: colorPalette.secondary[100],
    },
    tertiary: {
      main: colorPalette.tertiary[40],
      on: colorPalette.tertiary[100],
      container: colorPalette.tertiary[90],
      onContainer: colorPalette.tertiary[10],
      fixed: colorPalette.tertiary[90],
      onFixed: colorPalette.tertiary[10],
      surface: colorPalette.tertiary[100],
      onSurface: colorPalette.tertiary[100],
    },
    error: {
      main: colorPalette.error[40],
      on: colorPalette.error[100],
      container: colorPalette.error[90],
      onContainer: colorPalette.error[10],
    },
  },
};

export const materialDarkTheme: DefaultTheme = {
  ...baseTheme,
  colors: {
    ...baseTheme.colors,
    primary: {
      main: colorPalette.primary[80],
      on: colorPalette.primary[20],
      container: colorPalette.primary[30],
      onContainer: colorPalette.primary[90],
      fixed: colorPalette.primary[90],
      onFixed: colorPalette.primary[10],
      surface: colorPalette.primary[100],
      onSurface: colorPalette.primary[100],
    },
    secondary: {
      main: colorPalette.secondary[80],
      on: colorPalette.secondary[20],
      container: colorPalette.secondary[30],
      onContainer: colorPalette.secondary[90],
      fixed: colorPalette.secondary[90],
      onFixed: colorPalette.secondary[10],
      surface: colorPalette.secondary[100],
      onSurface: colorPalette.secondary[100],
    },
    tertiary: {
      main: colorPalette.tertiary[80],
      on: colorPalette.tertiary[20],
      container: colorPalette.tertiary[30],
      onContainer: colorPalette.tertiary[90],
      fixed: colorPalette.tertiary[90],
      onFixed: colorPalette.tertiary[10],
      surface: colorPalette.tertiary[100],
      onSurface: colorPalette.tertiary[100],
    },
    error: {
      main: colorPalette.error[80],
      on: colorPalette.error[20],
      container: colorPalette.error[30],
      onContainer: colorPalette.error[90],
    },
  },
};
