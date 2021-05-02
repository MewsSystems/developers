import React from "react";
import { ThemeProvider } from "styled-components";
import { theme } from "./styles/theme";
import { GlobalStyle } from "./styles/globalStyles";
import { AppRouter } from "./components/app-router";

export const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <AppRouter />
      <GlobalStyle />
    </ThemeProvider>
  );
};
