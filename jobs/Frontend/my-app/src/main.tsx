import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import './index.css';
import './main.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { MovieSearch } from './pages/MovieSearch';
import { MovieDetail } from './pages/MovieDetail';

const queryClient = new QueryClient();

const router = createBrowserRouter([
  {
    path: '/',
    element: <MovieSearch />,
  },
  {
    path: '/movie-detail/:movieId',
    element: <MovieDetail />,
  },
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <RouterProvider router={router} />
    </QueryClientProvider>
  </StrictMode>
);
