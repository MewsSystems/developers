import {
  fetchMoviePending,
  fetchMovieSuccess,
  fetchMovieError
} from "./movie.actions";

//Abort Controller
const abortController = new AbortController();
const signal = abortController.signal;

export const fetchMovie = movieId => {
  return dispatch => {
    dispatch(fetchMoviePending());
    fetch(
      `${process.env.REACT_APP_URI}movie/${movieId}?api_key=${process.env.REACT_APP_API_KEY}&language=en-US`,
      { signal: signal }
    )
      .then(response => response.json())
      .then(response => {
        dispatch(fetchMovieSuccess(response));
      })
      .catch(error => {
        dispatch(fetchMovieError(error));
      });
  };
};
