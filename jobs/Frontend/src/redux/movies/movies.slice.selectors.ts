import { RootState } from "../store";

export const moviesSelectors = {
  getMovies: (state: RootState) => state.movies.search.results,
  getCurrentSearchPage: (state: RootState) => state.movies.search.page,
  getQuery: (state: RootState) => state.movies.query,
  getSelectedMovie: (state: RootState) => state.movies.selectedMovie,
};
