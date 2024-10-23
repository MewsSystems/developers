import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { LoaderFunctionArgs, RouterProvider, createBrowserRouter, redirect } from 'react-router-dom';
import { useMemo } from 'react';
import { MoviesRoute } from './routes/Movies/movies.route.tsx';
import App from './App.tsx';
import { MovieDetailsRoute, movieDetailsLoader } from './routes/MovieDetails/movie-details.route.tsx';

const createAppRouter = (queryClient: QueryClient) =>
  createBrowserRouter([
    {
      path: '/',
      element: <App />,
      loader: () => redirect('/movies'),
    },
    {
      path: '/movies',
      element: <MoviesRoute />,
    },
    {
      path: '/movies/:movieId',
      element: <MovieDetailsRoute />,
      loader: (args: LoaderFunctionArgs) => movieDetailsLoader(queryClient)(args),
    },
  ]);

export const AppRouter = () => {
  const queryClient = useQueryClient();

  const router = useMemo(() => createAppRouter(queryClient), [queryClient]);

  return <RouterProvider router={router} />;
};
