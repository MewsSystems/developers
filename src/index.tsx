import React from "react"
import ReactDOM from "react-dom/client"
import { ToastContainer } from "react-toastify"
import { Provider } from "react-redux"
import { RouterProvider } from "react-router-dom"

import { store } from "./app/store"
import { Layout } from "./components/layout"
import { router } from "./router"

import "react-toastify/dist/ReactToastify.css"

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <Provider store={store}>
      <Layout>
        <RouterProvider router={router} />
      </Layout>
      <ToastContainer />
    </Provider>
  </React.StrictMode>,
)
