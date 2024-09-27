import { createSelector } from '@reduxjs/toolkit';
import { RootState } from './store';

const getMoviesSliceSelector = (state: RootState) => state.movies;

const getCurrentMoviesSelector = createSelector(
  [getMoviesSliceSelector],
  (state) => state.currentMovie,
);

const getMoviesSelector = createSelector(
  [getMoviesSliceSelector],
  (state) => state.movies,
);

const getSearchQuerySelector = createSelector(
  [getMoviesSliceSelector],
  (state) => state.searchQuery,
);

const getPageSelector = createSelector(
  [getMoviesSliceSelector],
  (state) => state.page,
);

const getNumberOfPagesSelector = createSelector(
  [getMoviesSliceSelector],
  (state) => state.numberOfPages,
);

export {
  getCurrentMoviesSelector,
  getMoviesSelector,
  getNumberOfPagesSelector,
  getPageSelector,
  getSearchQuerySelector,
};
