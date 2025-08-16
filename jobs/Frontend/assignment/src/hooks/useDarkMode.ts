import { DarkModeContext } from "@/theme/DarkModeProvider";
import { useContext } from "react";

export function useDarkMode() {
  return useContext(DarkModeContext);
}
