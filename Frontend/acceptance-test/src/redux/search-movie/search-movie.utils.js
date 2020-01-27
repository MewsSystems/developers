import {
  performSearchPending,
  performSearchSuccess,
  performSearchError
} from "./search-movie.actions";


const abortController = new AbortController();
const signal = abortController.signal;

export const perfomMovieSearch = match => {
  return dispatch => {
    dispatch(performSearchPending());
    fetch(
      `${process.env.REACT_APP_URI}search/movie?api_key=${process.env.REACT_APP_API_KEY}&language=en-US&query=${match.params.movieName}`,
      { signal: signal }
    )
      .then(response => response.json())
      .then(response => {
        dispatch(performSearchSuccess(response));
      })
      .catch(error => {
        dispatch(performSearchError(error));
      });
  };
};
