import { createBrowserRouter, RouterProvider } from "react-router-dom";
import SearchPage from "./pages/SearchPage";
import MovieDetailPage from "./pages/MovieDetailPage";
import AppContextProvider from "./contexts/AppContext";
import { movieDetailLoader } from "./pages/MovieDetailPage";
import "./App.css";
import RootPage from "./pages/RootPage";
import Error from "./pages/Error";

// TODO typescript config file - strict mode not true
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
        errorElement: <Error />,
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
