import "styled-components";
import type { AppTheme } from './app/styles/theme';

declare module "styled-components" {
  export interface DefaultTheme extends AppTheme { }
}