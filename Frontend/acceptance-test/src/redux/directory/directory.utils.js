import {
  fetchTopRatedMoviesPending,
  fetchTopRatedMoviesError,
  fetchTopRatedMoviesSuccess,
  fetchMoviePending,
  fetchMovieSuccess,
  fetchMovieError
} from "./directory.actions";

//Abort Controller
const abortController = new AbortController();
const signal = abortController.signal;

export const fetchTopRatedMovies = () => {
  return dispatch => {
    dispatch(fetchTopRatedMoviesPending());
    fetch(
      `${process.env.REACT_APP_URI}movie/top_rated?api_key=${process.env.REACT_APP_API_KEY}&language=en-US`,
      { signal: signal }
    )
      .then(res => res.json())
      .then(res => {
        dispatch(fetchTopRatedMoviesSuccess(res));
      })
      .catch(error => {
        dispatch(fetchTopRatedMoviesError(error));
      });
  };
};

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
