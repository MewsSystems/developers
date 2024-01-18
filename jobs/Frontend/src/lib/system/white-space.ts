import { Theme, RequiredTheme, ResponsiveValue, system, ThemeValue } from "styled-system";

export interface WhiteSpaceProps<ThemeType extends Theme = RequiredTheme, TVal = ThemeValue<'textStyles', ThemeType>> {
  whiteSpace?: ResponsiveValue<TVal, ThemeType> | undefined;
}

const whiteSpace = system({ whiteSpace: true });

export default whiteSpace;
