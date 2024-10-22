import { LoaderFunctionArgs, RouterProvider, createBrowserRouter, redirect } from 'react-router-dom';
import { Movies } from './routes/Movies/Movies.tsx';
import App from './App.tsx';
import { MovieDetails, movieDetailsLoader } from './routes/MovieDetails/MovieDetails.tsx';

export const createAppRouter = () =>
  createBrowserRouter([
    {
      path: '/',
      element: <App />,
      loader: () => redirect('/movies'),
    },
    {
      path: '/movies',
      element: <Movies />,
    },
    {
      path: '/movie-details/:movieId',
      element: <MovieDetails />,
      loader: (args: LoaderFunctionArgs) => movieDetailsLoader()(args),
    },
  ]);

export const AppRouter = () => {
  const router = createAppRouter();

  return <RouterProvider router={router} />;
};
