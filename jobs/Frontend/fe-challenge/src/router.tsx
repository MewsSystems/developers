import { Outlet, createBrowserRouter } from 'react-router-dom';
import Layout from '@/components/Layout';
import ErrorPage from '@/pages/error-page';
import Search from '@/pages/search';
import MovieDetail from '@/pages/movie-detail';

export const routes = [
  {
    path: '/',
    element: <Layout />,
    children: [
      {
        element: <Outlet />,
        errorElement: <ErrorPage />,
        children: [
          { index: true, element: <Search /> },
          {
            path: '/movie/:movieId',
            element: <MovieDetail />,
          },
        ],
      },
    ],
  },
];

const router = createBrowserRouter(routes);

export default router;
