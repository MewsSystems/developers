import React from 'react';
import ReactDOM from 'react-dom/client';
import {
  createBrowserRouter,
  Navigate,
  RouterProvider,
} from "react-router-dom";
import { store } from './Store/store'
import { Provider } from 'react-redux'
import {
  Error404Page,
  MovieDetailsPage,
  MovieListPage,
 } from './Features';
import App from './App';

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    errorElement: <App outlet={<Error404Page />} />,
    children: [
      {
        index: true,
        element: <Navigate to="/movies" replace />
      },
      {
        path: "/movies",
        element: <MovieListPage />,
        errorElement: <Error404Page />,
      },
      {
        path: "/movies/:id",
        element: <MovieDetailsPage />,
        errorElement: <Error404Page />,
      },
    ],
  },
]);

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <React.StrictMode>
    <Provider store={store}>
      <RouterProvider router={router} />
    </Provider>
  </React.StrictMode>
);
