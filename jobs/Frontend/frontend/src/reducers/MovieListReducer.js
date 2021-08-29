const DefaultState = {
  loading: false,
  data: [],
  errorMsg: '',
  // count: 0,
};

const MovieListReducer = (state = DefaultState, action) => {
  switch (action.type) {
    case 'MOVIE_LIST_LOADING':
      return {
        ...state,
        loading: true,
        errorMsg: '',
      };
    case 'MOVIE_LIST_FAIL':
      return {
        ...state,
        loading: false,
        errorMsg: 'unable to get movie',
      };

    case 'MOVIE_LIST_SUCCESS':
      return {
        ...state,
        loading: false,
        data: action.payload.results,
        errorMsg: '',
        // count: action.payload.count,
      };
    default:
      return state;
  }
};

export default MovieListReducer;
