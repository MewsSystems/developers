import 'styled-components';

const theme = {
  colors: {
    text: '#333',
    background: '#efefef',
    primary: '#1e3d59',
    primaryLight: '#4b6786',
    primaryDark: '#001730',
    secondary: '#f5f0e1',
    secondaryLight: '#fff',
    secondaryDark: '#c2beaf',
  },
  space: [
    0,
    '0.125rem', // 2px
    '0.25rem', // 4px
    '0.5rem', // 8px
    '1rem', // 16px
    '2rem', // 32px
    '4rem', // 64px
    '8rem', // 128px
    '16rem', // 256px
  ],
  fonts: {
    body: 'Helvetica Neue, Helvetica, Arial, sans-serif',
    heading: 'Helvetica Neue, Helvetica, Arial, sans-serif',
  },
  fontSizes: {
    xl: '4rem',
    l: '2rem',
    m: '1rem',
    s: '0.9rem',
    xs: '0.75rem',
  },
  fontWeights: {
    light: 200,
    normal: 400,
    bold: 700,
  },
  lineHeights: {
    body: 1.5,
    heading: 1.1,
  },
  borders: {
    none: 'none',
    thin: '1px solid',
  },
  radii: {
    none: 0,
    base: '0.25em',
    round: '99999em',
  },
  breakPoints: {
    mobileS: '320px',
    mobileM: '375px',
    mobileL: '425px',
    tablet: '768px',
    laptop: '1024px',
    laptopL: '1440px',
    desktop: '2560px',
  },
};

export type Theme = typeof theme;
export default theme;
