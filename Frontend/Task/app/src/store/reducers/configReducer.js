import * as actionTypes from '../actions/actionTypes';

const initialState = {};

const reducer = (state = initialState, action) => {
  switch (action.type) {
    case actionTypes.FETCH_CONFIG_SUCCESS:
      return action.payload;
    default:
      return state;
  }
};

export default reducer;
