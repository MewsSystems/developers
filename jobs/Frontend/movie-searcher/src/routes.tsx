import React from "react";
import { HomeRoute } from "./routes/home/home-route";
import { Layout } from "./routes/layout";
import { MovieRoute } from "./routes/movie/movie-route";

export const routes = [
  {
    path: "/",
    element: <Layout />,
    children: [
      {
        path: "",
        element: <HomeRoute />,
      },
      {
        path: "/movie/:movieId",
        element: <MovieRoute />,
      },
    ],
  },
];
