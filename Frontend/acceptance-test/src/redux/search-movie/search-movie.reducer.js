import SearchMovieActionTypes from "./search-movie.types";

const INITIAL_STATE = {
  isLoading: true,
  searchMovie: [],
  errors: null
};

const searchMovieReducer = (state = INITIAL_STATE, action) => {
  switch (action.type) {
    case SearchMovieActionTypes.PERFORM_SEARCH_PENDING:
      return {
        ...state,
        isLoading: true
      };
    case SearchMovieActionTypes.PERFORM_SEARCH_SUCCESS:
      return {
        ...state,
        isLoading: false,
        searchMovie: action.payload
      };
    case SearchMovieActionTypes.PERFORM_SEARCH_ERROR:
      return {
        ...state,
        isLoading: false,
        errors: action.payload
      };

    default:
      return state;
  }
};

export default searchMovieReducer;
