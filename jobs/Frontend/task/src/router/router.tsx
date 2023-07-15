import { createBrowserRouter } from "react-router-dom";
import { Route } from "src/router/Route";
import { ErrorPage } from "src/views/ErrorPage/ErrorPage";
import { MovieScreen } from "src/views/MovieScreen/MovieScreen";
import { MovieSearch } from "src/views/MovieSearch/MovieSearch";

export const router = createBrowserRouter([
  {
    path: Route.Home,
    element: <MovieSearch />,
    errorElement: <ErrorPage />,
  },
  {
    path: `${Route.Movie}/:movieId`,
    element: <MovieScreen />,
  },
]);
