import { RootState } from "../store";

export const moviesSelectors = {
  getMovies: (state: RootState) => state.movies.results,
  getQuery: (state: RootState) => state.movies.query,
  getSelectedMovie: (state: RootState) => state.movies.selectedMovie,
};
