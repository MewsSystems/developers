import { createBrowserRouter } from "react-router-dom"

import { MovieList } from "./pages/list"
import { MovieDetail } from "./pages/detail"

export const router = createBrowserRouter([
  {
    path: "/",
    element: <MovieList />,
  },
  {
    path: "movie/:id",
    element: <MovieDetail />,
  },
])
