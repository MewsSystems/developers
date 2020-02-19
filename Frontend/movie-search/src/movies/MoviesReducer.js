import { moviesActionTypes } from './MoviesConstants';

const initialState = {
  loading: false,
  movies: {
    results: []
  },
};

export default function moviesReducer(state = initialState, action) {
  const { type, payload = {} } = action;

  switch(type) {
    case moviesActionTypes.FETCH_MOVIES:
      return {
        ...state,
        loading: true,
      };

    case moviesActionTypes.FETCH_MOVIES_SUCCESS:
      const { movies } = payload || {};

      return {
        ...state,
        movies: movies,
        loading: false,
      };

     case moviesActionTypes.FETCH_MOVIES_ERROR:
      return {
        ...state,
        loading: false,
      };

    default:
      return state;
  }
}
