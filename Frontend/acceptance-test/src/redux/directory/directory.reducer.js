import DirectoryActionTypes from "./directory.types";

const INITIAL_STATE = {
  isLoading: true,
  movies: [],
  errors: null
};

const directoryReducer = (state = INITIAL_STATE, action) => {
  switch (action.type) {
    case DirectoryActionTypes.FETCH_TOP_RATED_MOVIES_PENDING:
      return {
        ...state,
        isLoading: true
      };
    case DirectoryActionTypes.FETCH_TOP_RATED_MOVIES_SUCCESS:
      return {
        ...state,
        isLoading: false,
        movies: action.payload
      };
    case DirectoryActionTypes.FETCH_TOP_RATED_MOVIES_ERROR:
      return {
        ...state,
        isLoading: false,
        errors: action.payload
      };

    default:
      return state;
  }
};

export default directoryReducer;
