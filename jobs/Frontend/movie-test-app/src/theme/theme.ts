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
export type Breakpoints = 'smallMobile' | 'mobile' | 'tablet' | 'smallDesktop' | 'desktop' | 'largeDesktop';

export type Theme = {
  colors: Colors;
  breakpoints: { [breakpoint in Breakpoints]: string };
};

export type Palette = {
  [themeColor in ThemeColors]: Colors;
};

export const palette: Palette = {
  blue: ThemeColorsBlue,
  red: ThemeColorsRed,
  purple: ThemeColorsPurple,
};

export const defaultTheme: Theme = {
  colors: ThemeColorsBlue,
  breakpoints: {
    smallMobile: '320px',
    mobile: '480px',
    tablet: '768px',
    smallDesktop: '1024px',
    desktop: '1200px',
    largeDesktop: '1440px',
  },
};
