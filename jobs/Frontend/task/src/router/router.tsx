import { createBrowserRouter } from "react-router-dom";
import { MovieSearch } from "src/views/MovieSearch/MovieSearch";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <MovieSearch />,
  },
]);
