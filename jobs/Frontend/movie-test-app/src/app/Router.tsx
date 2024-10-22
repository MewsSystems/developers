import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { LoaderFunctionArgs, RouterProvider, createBrowserRouter, redirect } from 'react-router-dom';
import { useMemo } from 'react';
import { Movies } from './routes/Movies/Movies.tsx';
import App from './App.tsx';
import { MovieDetails, movieDetailsLoader } from './routes/MovieDetails/MovieDetails.tsx';

export const createAppRouter = (queryClient: QueryClient) =>
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
      path: '/movies/:movieId',
      element: <MovieDetails />,
      loader: (args: LoaderFunctionArgs) => movieDetailsLoader(queryClient)(args),
    },
  ]);

export const AppRouter = () => {
  const queryClient = useQueryClient();

  const router = useMemo(() => createAppRouter(queryClient), [queryClient]);

  return <RouterProvider router={router} />;
};
