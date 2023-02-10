import { lazy, Suspense } from "react";
import { createBrowserRouter } from "react-router-dom";

import { SearchView, ErrorView } from "../views/public-api";

const MovieDetailView = lazy(() => import("../views/MovieDetailView"));

export const routes = createBrowserRouter([
  {
    path: "/",
    element: <SearchView />,
  },
  {
    path: "movie/:movieId",
    element: (
      <Suspense fallback={<>Loading...</>}>
        <MovieDetailView />
      </Suspense>
    ),
  },
  {
    path: "/error",
    element: <ErrorView />,
  },
  {
    path: "*",
    element: <SearchView />,
  },
]);
