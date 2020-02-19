import { moviesActionTypes } from './MoviesConstants';

export function fetchMovies(query, page) {
  return {
    type: moviesActionTypes.FETCH_MOVIES,
    payload: {
      query,
      page
    },
  };
}

export function fetchMoviesSuccess(movies) {
  return {
    type: moviesActionTypes.FETCH_MOVIES_SUCCESS,
    payload: {
      movies,
    },
  };
}

export function fetchMoviesError(error) {
  return {
    type: moviesActionTypes.FETCH_MOVIES_ERROR,
    payload: {
      error,
    },
  };
}
