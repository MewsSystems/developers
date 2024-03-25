import React from 'react'
import { createRoot } from 'react-dom/client'
import {RouterProvider} from "react-router-dom";
import {router} from "@/routes/router";
import "./index.css"
import {MovieSearchProvider} from "@/context/MovieSearchProvider";
import {createTheme, Grid, ThemeProvider} from "@mui/material";

const container = document.getElementById('root') as HTMLElement
const root = createRoot(container)

const darkTheme = createTheme({
    palette: {
        mode: 'dark',
    },
});


root.render(
  <React.StrictMode>
      <ThemeProvider theme={darkTheme}>
      <MovieSearchProvider>
          <Grid p={2}>
            <RouterProvider router={router} />
          </Grid>
      </MovieSearchProvider>
      </ThemeProvider>
  </React.StrictMode>
)
