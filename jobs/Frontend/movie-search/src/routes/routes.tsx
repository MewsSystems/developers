import Layout from "../layout/layout";
import MovieDetail from "../pages/movie-detail/movie-detail";
import NotFound from "../pages/not-found/not-found";
// eslint-disable-next-line import/named
import { RouteObject } from "react-router-dom";
import Home from "../pages/home/home";

export const routes: RouteObject[] = [
  {
    path: "/",
    element: <Layout />,
    children: [
      { index: true, 
        element: <Home /> },
      {
        path: "movies/:movieId",
        element: <MovieDetail />,
        children: [],
      },
      { path: "*", element: <NotFound /> },
    ],
  },
];
