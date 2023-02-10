import { RouterProvider } from "react-router-dom";
import { ThemeProvider } from "styled-components";
import { routes } from "./routes";
import "./App.css";
import { darkTheme, GlobalStyle } from "./theme";
import { AppContainer } from "./App.styled";

const App = () => {
  return (
    <ThemeProvider theme={darkTheme}>
      <GlobalStyle />
      <AppContainer>
        <RouterProvider router={routes} />
      </AppContainer>
    </ThemeProvider>
  );
};

export default App;
