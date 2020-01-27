import SearchMovieActionTypes from "./search-movie.types";

export const performSearchPending = () => ({
  type: SearchMovieActionTypes.PERFORM_SEARCH_PENDING
});

export const performSearchSuccess = searchMovie => ({
  type: SearchMovieActionTypes.PERFORM_SEARCH_SUCCESS,
  payload: searchMovie
});

export const performSearchError = error => ({
  type: SearchMovieActionTypes.PERFORM_SEARCH_ERROR,
  payload: error
});
