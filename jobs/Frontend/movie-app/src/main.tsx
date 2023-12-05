import React from "react"
import ReactDOM from "react-dom/client"
import { createBrowserRouter, RouterProvider } from "react-router-dom"
import { ThemeProvider } from "styled-components"
import { Provider } from "react-redux"
import { store } from "@/app/store"
import Root from "@/layout/Root"
import SearchPage from "@/pages/SearchPage"
import MovieDetail from "@/pages/MovieDetail"
import theme from "./theme"
import "./index.css"

const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    children: [
      {
        path: "/",
        element: <SearchPage />,
      },
      {
        path: "movies/:movieId",
        element: <MovieDetail />,
      },
    ],
  },
])

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <Provider store={store}>
      <ThemeProvider theme={theme}>
        <RouterProvider router={router} />
      </ThemeProvider>
    </Provider>
  </React.StrictMode>,
)
