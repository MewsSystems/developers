import { FETCH_RATES, FETCHING_RATES_ERROR } from '../actions/types';

export default (state = {}, action) => {
  switch (action.type) {
    case FETCH_RATES:
      return { ...state, rates: action.payload, fetchError: null };
    case FETCHING_RATES_ERROR:
      return { ...state, fetchError: action.payload };
    default:
      return state;
  }
};
