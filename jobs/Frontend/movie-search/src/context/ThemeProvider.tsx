"use client";

//setting the theme provider in this context instead of in the layout as we need to use "use client" and dont want to force the layout to be client side
import { theme } from "@/styles/StyledComponentsTheme";
import { ThemeProvider as StyledComponentsThemeProvider } from "styled-components";

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
