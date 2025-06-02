import {lazy} from 'react';
import {Route, Routes} from 'react-router-dom';

import {PageType, PathByPageType} from './constants';

const MoviesListPage = lazy(() => import('../pages/MoviesListPage/MoviesListPage'));
const MovieDetailsPage = lazy(() => import('../pages/MovieDetailsPage/MovieDetailsPage'));

export default function AppRoutes() {
  return (
    <Routes>
      <Route path={PathByPageType[PageType.MoviesListPage]} element={<MoviesListPage />} />
      <Route path={PathByPageType[PageType.MovieDetailsPage]} element={<MovieDetailsPage />} />
    </Routes>
  );
}
