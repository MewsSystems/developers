import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { LoaderFunctionArgs, RouterProvider, createBrowserRouter, redirect } from 'react-router-dom';
import { useMemo } from 'react';
import { MoviesRoute } from './routes/movies/movies.route.tsx';
import { MovieDetailsRoute, movieDetailsLoader } from './routes/movie-details/movie-details.route.tsx';

const createAppRouter = (queryClient: QueryClient) =>
  createBrowserRouter([
    {
      path: '/',
      element: <></>,
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
