import { createBrowserRouter, RouterProvider } from "react-router-dom";
import SearchPage from "./pages/SearchPage";
import MovieDetail from "./pages/MovieDetail";
import AppContextProvider from "./contexts/AppContext";
import "./App.css";

// TODO add error element
const router = createBrowserRouter([
  {
    path: "/",
    element: <SearchPage />,
  },
  {
    path: "/movie/:movieId",
    element: <MovieDetail />,
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