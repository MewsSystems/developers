import {
  GET_RATES_LOADING,
  GET_RATES_SUCCESS,
  GET_RATES_ERROR
} from './../actions/types';

const initialState = {
  rates: {},
  loading: false,
  trends: {}
};

export default (state = initialState, action) => {
  switch (action.type) {
    case GET_RATES_LOADING: {
      return {
        ...state,
        loading: true,
        error: null
      };
    }
    case GET_RATES_SUCCESS: {
      //const { rates } = action;
      //console.log(rates);
      return {
        ...state,
        loading: false,
        currencyRates: action.payload.rates
      };
    }
    case GET_RATES_ERROR: {
      return {
        ...state,
        loading: false,
        error: action.payload.error,
        rates: {}
      };
    }
    default:
      return state;
  }
};
