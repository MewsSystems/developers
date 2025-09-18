import { createBrowserRouter, Outlet } from "react-router-dom";
import MoviesPage from "@/features/movies/pages/MoviesPage";
import MovieDetailsPage from "@/features/movies/pages/MovieDetailsPage";
import App from "./App";
import { SuspenseBoundary } from "./components/SuspenseBoundary";
import { ErrorBoundary } from "./components/ErrorBoundary";

export const router = createBrowserRouter([
  {
    element: <ErrorBoundary>
      <Outlet />
    </ErrorBoundary>,
    children: [
      {
        path: "/",
        element: <App />,
        children: [
          { index: true, element: <MoviesPage /> },
          {
            path: "movie/:id",
            element: (
              <SuspenseBoundary label="Loading movie...">
                <MovieDetailsPage />
              </SuspenseBoundary>
            ),
          },
        ],
      },
    ],
  },
]);

