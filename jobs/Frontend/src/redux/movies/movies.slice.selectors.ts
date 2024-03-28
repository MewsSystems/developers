import { RootState } from "../store";

export const moviesSelectors = {
  getMovies: (state: RootState) => state.movies.search.results,
  hasResults: (state: RootState) => state.movies.search.results.length > 0,
  isLoading: (state: RootState) => state.movies.search.loading,
  getCurrentSearchPage: (state: RootState) => state.movies.search.page,
  getQuery: (state: RootState) => state.movies.query,
  getSelectedMovie: (state: RootState) => state.movies.selectedMovie,
  getSelectedMovieTitle: (state: RootState) =>
    state.movies.selectedMovie?.title ?? "Title not found",
  getSelectedMovieTagLine: (state: RootState) =>
    state.movies.selectedMovie?.tagline ?? null,
};
