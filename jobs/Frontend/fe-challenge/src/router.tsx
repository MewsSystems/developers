import { Outlet, createBrowserRouter } from 'react-router-dom';
import Layout from '@/components/Layout';
import ErrorPage from '@/pages/ErrorPage';
import Search from '@/pages/Search';
import Movie from '@/pages/Movie';

const router = createBrowserRouter([
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
            path: '/movie/:id',
            element: <Movie />,
          },
        ],
      },
    ],
  },
]);

export default router;
