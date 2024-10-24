const ThemeColorsPurple: Colors = {
  secondary: '#E4D6FA',
  primary: '#250A52',
};

const ThemeColorsRed: Colors = {
  secondary: '#FBD5E7',
  primary: '#6D0A37',
};

const ThemeColorsBlue: Colors = {
  secondary: '#D5F2FB',
  primary: '#084254',
};

export type Colors = {
  secondary: string;
  primary: string;
};

export type ThemeColors = 'blue' | 'red' | 'purple';

export type Theme = { [themeColor in ThemeColors]: Colors };

export const theme: Theme = {
  blue: ThemeColorsBlue,
  red: ThemeColorsRed,
  purple: ThemeColorsPurple,
};
