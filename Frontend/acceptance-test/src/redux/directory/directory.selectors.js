import { createSelector } from "reselect";

const selectDirectory = state => state.directory;

export const selectMovies = createSelector(
  [selectDirectory],
  directory => directory.movies
);

export const selectIsLoading = createSelector(
  [selectDirectory],
  directory => directory.isLoading
);

export const selectErrors = createSelector(
  [selectDirectory],
  directory => directory.errors
);
