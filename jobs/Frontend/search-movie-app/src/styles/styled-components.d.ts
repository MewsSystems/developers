import 'styled-components';

declare module 'styled-components' {
  export interface DefaultTheme {
    colors: {
      background: string;
      surface: string;
      primary: string;
      hoverPrimary: string;
      hoverSurface: string;
      error: string;
      onPrimary: string;
      onSurface: string;
    };
  }
}
