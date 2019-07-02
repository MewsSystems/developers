import * as actionTypes from '../actions/actionTypes';

const initialState = [];

const addFilter = (state, action) => {
  if (!state.includes(action.payload)) {
    return [...state, action.payload];
  } else {
    return state.filter(i => i !== action.payload);
  }
};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case actionTypes.FILTER:
      return addFilter(state, action);
    case actionTypes.RESET_FILTER:
      return initialState;
    default:
      return state;
  }
};

export default reducer;
