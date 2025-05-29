import {lazy} from 'react';
import {Route, Routes} from 'react-router-dom';
import {PageType, PathByPageType} from './constants.ts';

const MoviesListPage = lazy(() => import('../pages/MoviesListPage/MoviesListPage.tsx'));
const MovieDetailsPage = lazy(() => import('../pages/MovieDetailsPage/MovieDetailsPage.tsx'));

export default function AppRoutes() {
  return (
    <Routes>
      <Route path={PathByPageType[PageType.MoviesListPage]} element={<MoviesListPage />} />
      <Route path={PathByPageType[PageType.MovieDetailsPage]} element={<MovieDetailsPage />} />
    </Routes>
  );
}
