import { Outlet, createBrowserRouter } from 'react-router-dom';
import Layout from '@/components/Layout';
import ErrorPage from '@/pages/error-page-tmp';
import Search from '@/pages/search-tmp';
import MovieDetail from '@/pages/movie-detail-tmp';

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
