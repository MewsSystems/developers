import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import MovieSearch from "./pages/MovieSearch.tsx";
import MovieDetail from "./pages/MovieDetail.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "",
        element: <MovieSearch />,
      },
      {
        path: "movie-detail/:id",
        element: <MovieDetail />,
      },
      {
        path: "*", // This route will match any path not previously matched
        element: <div> Not found</div>, // This is your 404 page
      },
    ],
  },
]);

ReactDOM.createRoot(
  document.getElementById("root")!
).render(<RouterProvider router={router} />);
