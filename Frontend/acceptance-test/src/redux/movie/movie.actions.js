import MovieActionTypes from "./movie.types";

export const fetchMoviePending = () => ({
  type: MovieActionTypes.FETCH_MOVIE_PENDING
});

export const fetchMovieSuccess = movie => ({
  type: MovieActionTypes.FETCH_MOVIE_SUCCESS,
  payload: movie
});

export const fetchMovieError = error => ({
  type: MovieActionTypes.FETCH_MOVIE_ERROR,
  payload: error
});
