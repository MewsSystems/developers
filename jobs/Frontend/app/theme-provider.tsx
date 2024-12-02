"use client";

import { ThemeProvider as StyledComponentsThemeProvider } from "styled-components";
import theme from "@/styles/theme";

export default function ThemeProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <StyledComponentsThemeProvider theme={theme}>
      {children}
    </StyledComponentsThemeProvider>
  );
}
