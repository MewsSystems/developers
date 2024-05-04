import "styled-components";
import { theme } from "../styles/theme";

type Theme = typeof theme;

// Allows for IntelliSense with TypeScript
declare module "styled-components" {
  export interface DefaultTheme extends Theme {
    primary: {
      text: string;
      background: string;
      border: string;
      button: string;
    };
  }
}
