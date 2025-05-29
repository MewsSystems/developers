import {QueryClient, QueryClientProvider} from '@tanstack/react-query';
import {lazy} from 'react';
import {Route, Routes} from 'react-router-dom';

const MoviesListPage = lazy(() => import('./pages/MoviesListPage/MoviesListPage.tsx'));
const MovieDetailsPage = lazy(() => import('./pages/MovieDetailsPage/MovieDetailsPage.tsx'));

const queryClient = new QueryClient();

export default function AppRoutes() {
  return (
    <QueryClientProvider client={queryClient}>
      <Routes>
        <Route path="/" element={<MoviesListPage />} />
        <Route path="movies/:id" element={<MovieDetailsPage />} />
      </Routes>
    </QueryClientProvider>
  );
}
