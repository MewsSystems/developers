import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { SearchPage, DetailsPage } from "./pages";
import { ThemeProvider } from "styled-components";
import { materialDarkTheme, materialLightTheme } from "./theme/themes";
import { withDarkMode } from "./theme/DarkModeProvider";
import { useDarkMode } from "./hooks";

const router = createBrowserRouter([
  {
    path: "/",
    element: <SearchPage />,
  },
  {
    path: "/movie/:id",
    element: <DetailsPage />,
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

export default withDarkMode(App);
