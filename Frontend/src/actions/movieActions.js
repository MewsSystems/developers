import {
  GET_MOVIES,
  GET_MOVIE,
  SET_LOADING,
  MOVIES_ERROR, SET_SEARCH_TERM
} from './types';

const baseUrl = `https://api.themoviedb.org/3`;

// Get now running movies
export const searchMovies = (text, page = 1) => async dispatch => {
  try {
    setLoading();

    let data;

    if (text !== '') {
      const res = await fetch(`${baseUrl}/search/movie?api_key=${process.env.REACT_APP_TMDB_KEY}&query=${text}&page=${page}`);
      data = await res.json();
    } else {
      data = {
        results: []
      }
    }

    dispatch({
      type: GET_MOVIES,
      payload: data
    })
  } catch (error) {
    dispatch({
      type: MOVIES_ERROR,
      payload: error.response.status
    })
  }
};

// get movie by id
export const getMovie = id => async dispatch => {
  try {
    setLoading();

    const res = await fetch(`${baseUrl}/movie/${id}?api_key=${process.env.REACT_APP_TMDB_KEY}`);
    const data = await res.json();


    dispatch({
      type: GET_MOVIE,
      payload: data
    })
  } catch (error) {
    dispatch({
      type: MOVIES_ERROR,
      payload: error.response.status
    })
  }
};

// set loading to true
export const setSearchTerm = searchTerm => {
  return {
    type: SET_SEARCH_TERM,
    payload: searchTerm
  }
};

// set loading to true
export const setLoading = () => {
  return {
    type: SET_LOADING
  }
};
