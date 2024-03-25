import React from 'react'
import { createRoot } from 'react-dom/client'
import {RouterProvider} from "react-router-dom";
import {router} from "@/routes/router";
import "./index.css"
import {MovieSearchProvider} from "@/context/MovieSearchProvider";

const container = document.getElementById('root') as HTMLElement
const root = createRoot(container)

root.render(
  <React.StrictMode>
      <MovieSearchProvider>
          <RouterProvider router={router} />
      </MovieSearchProvider>
  </React.StrictMode>
)
