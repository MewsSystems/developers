import 'styled-components';

declare module 'styled-components' {
  export interface DefaultTheme {
    colors: {
      background: string;
      surface: string;
      primary: string;
      secondary: string;
      disabled: string;
      hoverPrimary: string;
      hoverSecondary: string;
      hoverSurface: string;
      error: string;
      onPrimary: string;
      onSurface: string;
      onDisabled: string;
    };
  }
}
