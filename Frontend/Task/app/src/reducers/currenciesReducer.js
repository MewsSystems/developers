import { FETCH_CURRENCIES, SET_FILTER_VALUE } from '../actions/types';

export default (state = {}, action) => {
  switch (action.type) {
    case FETCH_CURRENCIES:
      return { ...state, currencies: action.payload };
    case SET_FILTER_VALUE:

      return { ...state,  filteredValue: action.payload };
    default:
      return state;
  }
};
