import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./index.css";
import Layout from "./app/layouts/Layout";
import HomePage from "./app/routes/HomePage";
import MoviePage from "./app/routes/MoviePage";
import RouteErrorElement from "./app/errors/RouteErrorElement";
import NotFoundPage from "./app/routes/NotFoundPage";

const router = createBrowserRouter([
    {
        element: <Layout />,
        errorElement: <RouteErrorElement />, // global error UI
        children: [
            { path: "/", element: <HomePage /> },
            { path: "/movie/:id", element: <MoviePage /> },
            { path: "*", element: <NotFoundPage /> },
        ],
    },
]);

createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <RouterProvider router={router} />
    </StrictMode>
);
