import MovieActionTypes from "./movie.types";

const INITIAL_STATE = {
  isLoading: true,
  movie: [],
  errors: null
};

const movieReducer = (state = INITIAL_STATE, action) => {
  switch (action.type) {
    case MovieActionTypes.FETCH_MOVIE_PENDING:
      return {
        ...state,
        isLoading: true
      };
    case MovieActionTypes.FETCH_MOVIE_SUCCESS:
      return {
        ...state,
        isLoading: false,
        movie: action.payload
      };
    case MovieActionTypes.FETCH_MOVIE_ERROR:
      return {
        ...state,
        isLoading: false,
        errors: action.payload
      };

    default:
      return state;
  }
};

export default movieReducer;
