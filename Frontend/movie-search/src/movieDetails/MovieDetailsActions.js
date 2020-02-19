import { movieDetailsActionTypes } from './MovieDetailsConstants';

export function fetchMovieDetails(id) {
  return {
    type: movieDetailsActionTypes.FETCH_MOVIE_DETAILS,
    payload: {
      id,
    },
  };
}

export function fetchMovieDetailsSuccess(movie) {
  return {
    type: movieDetailsActionTypes.FETCH_MOVIE_DETAILS_SUCCESS,
    payload: {
      movie,
    },
  };
}

export function fetchMovieDetailsError(error) {
  return {
    type: movieDetailsActionTypes.FETCH_MOVIE_DETAILS_ERROR,
    payload: {
      error,
    },
  };
}
