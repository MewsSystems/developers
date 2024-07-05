import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import { Home } from './routes/Home';
import reportWebVitals from './reportWebVitals';
import {
  createBrowserRouter,
  RouterProvider,
  redirect,
} from 'react-router-dom';
import { ErrorPage } from './ErrorPage';
import { Details } from './routes/Details';
import { Provider } from 'react-redux';
import { store } from './redux/store';

const redirectLoader = () => {
  return redirect('/search');
};

const router = createBrowserRouter([
  {
    path: '/',
    loader: redirectLoader,
    errorElement: <ErrorPage />,
  },
  { path: 'search', element: <Home />, errorElement: <ErrorPage /> },
  { path: 'details', element: <Details /> },
]);

const root = document.getElementById('root');

if (root) {
  ReactDOM.createRoot(root).render(
    <React.StrictMode>
      <Provider store={store}>
        <RouterProvider router={router} />
      </Provider>
    </React.StrictMode>,
  );
}

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
