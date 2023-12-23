import { FC, PropsWithChildren } from "react";
import {
  RouterProvider,
  createBrowserRouter
} from "react-router-dom";
import MovieDetails from "../../movieDetails/MovieDetails";
import Movies from "../../movies/Movies";
import Routes from "../enums/Routes";

const AppRouter: FC<PropsWithChildren> = () => {
  return <RouterProvider router={router} />;
};

export default AppRouter;

const router = createBrowserRouter([
  {
    path: Routes.Movies,
    element: <Movies />,
  },
  {
    path: Routes.MovieDetails,
    element: <MovieDetails />,
  },
]);
