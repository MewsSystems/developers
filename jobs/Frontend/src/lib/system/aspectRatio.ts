import { RequiredTheme, ResponsiveValue, system, Theme } from '@tradersclub/styled-system';

export interface AspectRatioProps<ThemeType extends Theme = RequiredTheme> {
  aspectRatio?: ResponsiveValue<string, ThemeType> | undefined;
}

export const aspectRatio = system({
  aspectRatio: true,
});
