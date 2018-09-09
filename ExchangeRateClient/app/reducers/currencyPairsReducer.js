import {
  GET_CURRENCY_PAIRS_LOADING,
  GET_CURRENCY_PAIRS_SUCCESS,
  GET_CURRENCY_PAIRS_ERROR
} from './../actions/types';

const initialState = {
  currencyPairs: {},
  loading: false,
  error: null
};

export default (state = initialState, action) => {
  switch (action.type) {
    case GET_CURRENCY_PAIRS_LOADING: {
      return {
        ...state,
        loading: true,
        error: null
      };
    }
    case GET_CURRENCY_PAIRS_SUCCESS: {
      //const { currencyPairs } = action;
      //console.log(currencyPairs);
      return {
        ...state,
        loading: false,
        currencyPairs: action.payload.currencyPairs
      };
    }
    case GET_CURRENCY_PAIRS_ERROR: {
      return {
        ...state,
        loading: false,
        error: action.payload.error,
        items: {}
      };
    }
    default:
      return state;
  }
};
