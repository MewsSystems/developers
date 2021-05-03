import React from "react";
import { ThemeProvider } from "styled-components";
import { theme } from "./styles/theme";
import { GlobalStyle } from "./styles/globalStyles";
import { AppRouter } from "./components/app-router";
import { Provider } from "react-redux";
import { store } from "./redux/state";

export const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <Provider store={store}>
        <AppRouter />
      </Provider>
      <GlobalStyle />
    </ThemeProvider>
  );
};
