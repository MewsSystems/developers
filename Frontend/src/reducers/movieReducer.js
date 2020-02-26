import {
  GET_MOVIE,
  GET_MOVIES,
  MOVIES_ERROR,
  SET_LOADING,
  SET_SEARCH_TERM
} from '../actions/types';

const initialState = {
  movies: [],
  selectedMovie: null,
  page: null,
  total_pages: null,
  loading: false,
  error: false,
  searchTerm: null
};

export default (state = initialState, action) => {
  switch (action.type) {
    case GET_MOVIE:
      return {
        ...state,
        selectedMovie: action.payload,
        loading: false
      };
    case GET_MOVIES:
      return {
        ...state,
        movies: action.payload.results,
        page: action.payload.page,
        total_pages: action.payload.total_pages,
        total_results: action.payload.total_results,
        loading: false
      };
    case SET_SEARCH_TERM:
      return {
        ...state,
        searchTerm: action.payload
      };
    case MOVIES_ERROR:
      return {
        ...state,
        error: action.payload
      };
    case SET_LOADING:
      return {
        ...state,
        loading: true
      };
    default:
      return state;
  }
}
