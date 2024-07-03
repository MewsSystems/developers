import React from "react";
import * as ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import SearchPage from "./pages/SearchPage";
import MovieDetailPage from "./pages/MovieDetailPage";
import Header from "./components/Header";
import Footer from "./components/Footer";
import GlobalStyle from "./globalStyles";

console.log("load the app");
const router = createBrowserRouter([
  {
    path: "/",
    element: <SearchPage />,
  },
  {
    path: "/movie/:id",
    element: <MovieDetailPage />,
  },
]);

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement,
);
root.render(
  <React.StrictMode>
    <GlobalStyle />
    <Header />
    <RouterProvider router={router} />
    <Footer />
  </React.StrictMode>,
);
