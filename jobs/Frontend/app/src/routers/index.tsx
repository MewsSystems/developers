import { lazy } from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import ErrorBoundary from '../components/ErrorBoundary';

const MoviesPage = lazy(() => import('../features/movies/pages/MoviesPage'));
const MovieDetailPage = lazy(() => import('../features/movies/pages/MovieDetailPage'));
const NotFound = lazy(() => import('../components/NotFound'));

export default function AppRouter() {
  return (
    <Router>
      <ErrorBoundary>
        <Routes>
          <Route path="/" element={<MoviesPage />} />
          <Route path="/movie/:id" element={<MovieDetailPage />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </ErrorBoundary>
    </Router>
  );
}
