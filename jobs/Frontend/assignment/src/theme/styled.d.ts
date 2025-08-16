import {} from "styled-components";
import { GlobalTheme } from "./types";

declare module "styled-components" {
  export interface DefaultTheme extends GlobalTheme {}
}
