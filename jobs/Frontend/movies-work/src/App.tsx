import { createBrowserRouter, RouterProvider } from "react-router-dom";
import SearchPage from "./pages/SearchPage";
import MovieDetailPage from "./pages/MovieDetailPage";
import AppContextProvider from "./contexts/AppContext";
import { movieDetailLoader } from "./pages/MovieDetailPage";
import "./App.css";
import RootPage from "./pages/RootPage";

// TODO add error element
const router = createBrowserRouter([
  {
    path: "/",
    element: <RootPage />,
    children: [
      { path: "", element: <SearchPage /> },
      {
        path: "/movie/:movieID",
        element: <MovieDetailPage />,
        loader: movieDetailLoader,
      },
    ],
  },
]);

function App() {
  return (
    <>
      <AppContextProvider>
        <RouterProvider router={router} />
      </AppContextProvider>
    </>
  );
}

export default App;
