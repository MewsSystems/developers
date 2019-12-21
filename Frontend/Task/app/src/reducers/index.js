import * as types from "../actions/types";

const initialState = {
  status: false,
  config: {},
  filter: {},
  rates: {},
  data: [],
  combinedRates: []
};

export default function ratesReducer(state = initialState, action) {
  switch (action.type) {
    case types.GET_CONFIG:
      return Object.assign({}, state, {
        config: action.config
      });
    case types.GET_RATES:
      return Object.assign({}, state, {
        rates: action.rates
      });
    case types.GET_COMBINED_RATES:
      return Object.assign({}, state, {
        combinedRates: action.combinedRates
      });
    case types.GET_RATES_DATA:
      return Object.assign({}, state, {
        data: action.data
      });
    case types.SET_LOADING_STATUS:
      return Object.assign({}, state, {
        status: action.status
      });
    case types.CHANGE_FILTER:
      return Object.assign({}, state, {
        filter: action.filter
      });

    default:
      return state;
  }
}
