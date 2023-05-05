import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { Provider } from "react-redux";
import { ThemeProvider } from "styled-components";
import { routes } from "./routes";
import { basicStyle, customStyle } from "./styleVariables";
import { store } from "./store";

const router = createBrowserRouter(routes);
const theme = {
  ...basicStyle,
  ...customStyle,
};

function App() {
  return (
    <Provider store={store}>
      <ThemeProvider theme={theme}>
        <RouterProvider router={router} />
      </ThemeProvider>
    </Provider>
  );
}

export default App;
