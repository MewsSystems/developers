import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { LoaderFunctionArgs, RouterProvider, createBrowserRouter } from 'react-router-dom';
import { useMemo } from 'react';
import { Movies, moviesLoader } from './routes/Movies/Movies.tsx';

export const createAppRouter = (queryClient: QueryClient) =>
  createBrowserRouter([
    {
      path: '/',
      index: true,
      element: <Movies />,
      loader: (args: LoaderFunctionArgs) => moviesLoader(queryClient)(args),
    },
    {
      path: '/movies',
      element: <Movies />,
      loader: (args: LoaderFunctionArgs) => moviesLoader(queryClient)(args),
    },
  ]);

export const AppRouter = () => {
  const queryClient = useQueryClient();

  const router = useMemo(() => createAppRouter(queryClient), [queryClient]);

  return <RouterProvider router={router} />;
};
