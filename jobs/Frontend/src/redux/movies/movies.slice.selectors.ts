import { RootState } from "../store";

export const moviesSelectors = {
  getMovies: (state: RootState) => state.movies.search.results,
  hasResults: (state: RootState) => state.movies.search.results.length > 0,
  isLoading: (state: RootState) => state.movies.search.loading,
  getCurrentSearchPage: (state: RootState) => state.movies.search.page,
  getQuery: (state: RootState) => state.movies.query,
  getSelectedMovie: (state: RootState) => state.movies.selectedMovie,
  getSearchPage: (state: RootState) => state.movies.search.page ?? 0,
  getTotalPages: (state: RootState) => state.movies.search.total_pages ?? 0,
  getSelectedMovieTitle: (state: RootState) =>
    state.movies.selectedMovie?.title ?? "Title not found",
  getSelectedMovieTagLine: (state: RootState) =>
    state.movies.selectedMovie?.tagline ?? null,
};
