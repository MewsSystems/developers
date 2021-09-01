import axios from 'axios';

const apiKey = process.env.REACT_APP_API;

export const GetAllMovie = (searchTerm) => async (dispatch) => {
  try {
    dispatch({
      type: 'All_MOVIE_LOADING',
    });

    const res = await axios.get(
      `https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=${searchTerm}
    `,
    );

    dispatch({
      type: 'All_MOVIE_SUCCESS',
      payload: res.data,
    });
  } catch (e) {
    dispatch({
      type: 'All_MOVIE_FAIL',
    });
  }
};

export const GetMovieList = (pageNumber, searchTerm) => async (dispatch) => {
  try {
    dispatch({
      type: 'MOVIE_LIST_LOADING',
    });

    const res = await axios.get(
      `https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=${searchTerm}&page=${pageNumber}
    `,
    );

    dispatch({
      type: 'MOVIE_LIST_SUCCESS',
      payload: res.data,
      pageNumber: pageNumber,
      searchTerm: searchTerm,
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
      movieId: movieId,
    });
  } catch (e) {
    dispatch({
      type: 'MOVIE_MULTIPLE_FAIL',
    });
  }
};
