const DefaultState = {
  loading: false,
  data: [],
  errorMsg: '',
  count: 0,
};

const AllMovieReducer = (state = DefaultState, action) => {
  switch (action.type) {
    case 'All_MOVIE_LOADING':
      return {
        ...state,
        loading: true,
        errorMsg: '',
      };
    case 'All_MOVIE_FAIL':
      return {
        ...state,
        loading: false,
        errorMsg: 'unable to get movie',
      };

    case 'All_MOVIE_SUCCESS':
      return {
        ...state,
        loading: false,
        data: action.payload.results,
        errorMsg: '',
        count: action.payload.total_results,
        pages: action.payload.total_pages,
      };
    default:
      return state;
  }
};

export default AllMovieReducer;
