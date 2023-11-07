import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { Search, Details } from "./pages";
import { ThemeProvider } from "styled-components";
import { materialDarkTheme, materialLightTheme } from "./theme/themes";
import { usePrefersColorTheme } from "./hooks";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Search />,
  },
  {
    path: "/details/:id",
    element: <Details />,
  },
]);

function App() {
  const isDarkMode = usePrefersColorTheme() === "dark";

  return (
    <ThemeProvider theme={isDarkMode ? materialDarkTheme : materialLightTheme}>
      <RouterProvider router={router} />
    </ThemeProvider>
  );
}

export default App;
