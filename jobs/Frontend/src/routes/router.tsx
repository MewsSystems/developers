import {createBrowserRouter} from "react-router-dom";
import {MoviesListView, MovieDetailsView} from "@/views";
import React from "react";

export const router = createBrowserRouter([
    {
        path: "/movie/:id",
        element: <MovieDetailsView />,
    },
    {
        path: "/",
        element: <MoviesListView />,
    }
])
