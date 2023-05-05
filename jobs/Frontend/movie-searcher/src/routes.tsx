import { DashboardPage } from "./pages/DashboardPage";
import { MovieDetailsPage } from "./pages/MovieDetailsPage";
import { NotFoundPage } from "./pages/NotFoundPage";
import { RootPage } from "./pages/Root";

export const routes = [
  {
    path: "/",
    element: <RootPage />,
    children: [
      {
        element: <DashboardPage />,
        index: true,
      },
      {
        path: "movie-info/:movieId",
        element: <MovieDetailsPage />,
      },
      {
        path: "*",
        element: <NotFoundPage />,
      },
    ],
  },
];
