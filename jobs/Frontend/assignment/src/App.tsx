import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { Search, Details } from "./pages";
import { ThemeProvider } from "styled-components";
import { materialDarkTheme, materialLightTheme } from "./theme/themes";
import { DarkModeProvider } from "./theme/DarkModeProvider";
import { useDarkMode } from "./hooks";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Search />,
  },
  {
    path: "/movie/:id",
    element: <Details />,
  },
]);

function App() {
  const { darkMode } = useDarkMode();

  return (
    <ThemeProvider theme={darkMode ? materialDarkTheme : materialLightTheme}>
      <RouterProvider router={router} />
    </ThemeProvider>
  );
}

function WithDarkModeProvider() {
  return (
    <DarkModeProvider>
      <App />
    </DarkModeProvider>
  );
}

export default WithDarkModeProvider;
