import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import './main.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MovieSearchPage } from './pages/MovieSearchPage';
import { MovieDetailPage } from './pages/MovieDetailPage';
import { NotFoundPage } from './pages/NotFoundPage';

const queryClient = new QueryClient();

const router = createBrowserRouter([
  {
    path: '/',
    element: <MovieSearchPage />,
    errorElement: <NotFoundPage />,
  },
  {
    path: '/movie-detail/:movieId',
    element: <MovieDetailPage />,
  },
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </StrictMode>
);
