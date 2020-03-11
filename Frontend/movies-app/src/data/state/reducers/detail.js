import Actions from './../actionTypes';

const detail = (state, action) => {
  switch (action.type) {
    case Actions.LOAD_DETAIL:
      return { ...state, isLoading: true, id: action.id, item: null };

    case Actions.LOAD_DETAIL_ERROR:
      return { ...state, isLoading: false, error: action.error, item: null };

    case Actions.LOAD_DETAIL_SUCCESS:
      return {
        ...state,
        isLoading: false,
        error: '',
        item: action.data
      };

    default:
      return { ...state };
  }
};

export default detail;
