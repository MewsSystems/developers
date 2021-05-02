import React from "react";
import { SearchView } from "./views/search-view";
import { ThemeProvider } from "styled-components";
import { theme } from "./styles/theme";
import { GlobalStyle } from "./styles/globalStyles";

export const App = () => {
  return (
    <ThemeProvider theme={theme}>
      <div className="App">
        <SearchView />
      </div>
      <GlobalStyle />
    </ThemeProvider>
  );
};
