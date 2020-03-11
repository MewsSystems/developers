import Actions from './../actionTypes';

const movies = (state, action) => {
  switch (action.type) {
    case Actions.LOAD_MOVIES:
      return { ...state, searchTerm: action.searchTerm, page: action.page, isLoading: true, items: null };

    case Actions.LOAD_MOVIES_PAGE:
      return { ...state, page: action.page, isLoading: true, items: null };

    case Actions.LOAD_MOVIES_ERROR:
      return { ...state, isLoading: false, error: action.error, items: null };

    case Actions.LOAD_MOVIES_SUCCESS:
      return {
        ...state,
        isLoading: false,
        error: '',
        currentPage: action.data.page,
        totalPages: action.data.total_pages,
        items: action.data.results
      };

    default:
      return { ...state };
  }
};

export default movies;
