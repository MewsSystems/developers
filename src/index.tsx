import React from "react"
import ReactDOM from "react-dom/client"
import { ToastContainer } from "react-toastify"
import { Provider } from "react-redux"
import { RouterProvider } from "react-router-dom"
import { createGlobalStyle } from "styled-components"

import { store } from "./app/store"
import { Layout } from "./components/layout"
import { router } from "./router"

import "react-toastify/dist/ReactToastify.css"

const GlobalStyle = createGlobalStyle`
  * {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
    font-family: Helvetica, Arial, sans-serif;
  }
`

ReactDOM.createRoot(document.getElementById("root")!).render(
  <>
    <GlobalStyle />
    <React.StrictMode>
      <Provider store={store}>
        <Layout>
          <RouterProvider router={router} />
        </Layout>
        <ToastContainer />
      </Provider>
    </React.StrictMode>
  </>,
)
