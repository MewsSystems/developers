import { SET_FILTER_PARAMS } from 'Actions/types';

const initialState = {
  sortParams: null
};

const filterReducer = (state = initialState, action) => {
  switch (action.type) {
    case SET_FILTER_PARAMS:
      return {
        ...state,
        sortParams: action.payload
      };

    default:
      return state;
  }
};

export default filterReducer;
