import "styled-components"
import type { theme } from "./theme"

type Theme = typeof theme

declare module "styled-components" {
  export interface DefaultTheme extends Theme {}
}
