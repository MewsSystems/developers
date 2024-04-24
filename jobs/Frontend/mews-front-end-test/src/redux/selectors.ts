import { createSelector } from '@reduxjs/toolkit';
import { RootState } from './store';

const getMoviesSelector = (state: RootState) => state.movies;

const getCurrentMoviesSelector = createSelector(
  [getMoviesSelector],
  (state) => state.currentMovie,
);

export { getCurrentMoviesSelector };
