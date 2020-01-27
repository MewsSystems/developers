import DirectoryActionTypes from "./directory.types";

export const fetchTopRatedMoviesPending = () => ({
  type: DirectoryActionTypes.FETCH_TOP_RATED_MOVIES_PENDING
});

export const fetchTopRatedMoviesSuccess = movies => ({
  type: DirectoryActionTypes.FETCH_TOP_RATED_MOVIES_SUCCESS,
  payload: movies
});

export const fetchTopRatedMoviesError = error => ({
  type: DirectoryActionTypes.FETCH_TOP_RATED_MOVIES_ERROR,
  payload: error
});

export const fetchMoviePending = () => ({
  type: DirectoryActionTypes.FETCH_MOVIE_PENDING
});

export const fetchMovieSuccess = movie => ({
  type: DirectoryActionTypes.FETCH_MOVIE_SUCCESS,
  payload: movie
});

export const fetchMovieError = error => ({
  type: DirectoryActionTypes.FETCH_MOVIE_ERROR,
  payload: error
});
