import { FETCH_RATES, FETCHING_RATES_ERROR } from '../actions/types';

const initialState = {
  rates: {},
  oldRates: {},
  fetchError: null,
  lastFetched: null,
};

export default (state = initialState, action) => {
  switch (action.type) {
    case FETCH_RATES:
      if (!action.payload) {
        return state;
      }
      return {
        ...state,
        rates: action.payload,
        oldRates: state.rates,
        fetchError: null,
        lastFetched: Date.now(),
      };
    case FETCHING_RATES_ERROR:
      return { ...state, fetchError: action.payload };
    default:
      return state;
  }
};
