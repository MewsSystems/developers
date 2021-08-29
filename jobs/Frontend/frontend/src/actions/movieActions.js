import axios from 'axios';

const apiKey = process.env.REACT_APP_API;

export const GetMovieList = (searchTerm) => async (dispatch) => {
  try {
    dispatch({
      type: 'MOVIE_LIST_LOADING',
    });

    const res = await axios.get(
      `https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=${searchTerm}
    `,
    );

    dispatch({
      type: 'MOVIE_LIST_SUCCESS',
      payload: res.data,
    });
  } catch (e) {
    dispatch({
      type: 'MOVIE_LIST_FAIL',
    });
  }
};

export const GetMovie = (movieId) => async (dispatch) => {
  try {
    dispatch({
      type: 'MOVIE_MULTIPLE_LOADING',
    });

    const res = await axios.get(
      `https://api.themoviedb.org/3/movie/${movieId}?api_key=${apiKey}`,
    );

    dispatch({
      type: 'MOVIE_MULTIPLE_SUCCESS',
      payload: res.data,
      Id: movieId,
    });
  } catch (e) {
    dispatch({
      type: 'MOVIE_MULTIPLE_FAIL',
    });
  }
};
