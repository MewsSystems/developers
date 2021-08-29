import axios from 'axios';

export const GetMovieList = (searchTerm) => async (dispatch) => {
  const apiKey = process.env.REACT_APP_API;

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
