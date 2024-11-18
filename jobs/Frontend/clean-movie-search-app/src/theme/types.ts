export interface Theme {
  name: 'light' | 'dark';
  colors: {
    primary: string;
    secondary: string;
    background: string;
    surface: string;
    text: {
      primary: string;
      secondary: string;
    };
    button: {
      background: string;
      hover: string;
      text: string;
      inverseText: string;
    };
    card: {
      background: string;
      shadow: string;
      hoverShadow: string;
    };
    rating: {
      background: string;
      text: string;
      highRating: {
        border: string;
        text: string;
      };
    };
    input: {
      background: string;
      border: string;
      focusBorder: string;
    };
  };
  spacing: {
    xs: string;
    sm: string;
    md: string;
    lg: string;
    xl: string;
  };
  borderRadius: {
    sm: string;
    md: string;
    lg: string;
  };
}
