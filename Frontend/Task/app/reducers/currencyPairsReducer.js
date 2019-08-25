import {
  GET_CURRENCY_PAIRS_STARTED,
  GET_CURRENCY_PAIRS_SUCCESS,
  GET_CURRENCY_PAIRS_FAILURE
} from 'Actions/types';

const initialState = {
  loading: false,
  currencyPairs: {}
};

const currencyPairsReducer = (state = initialState, action) => {
  switch (action.type) {
    case GET_CURRENCY_PAIRS_STARTED:
      return {
        ...state,
        loading: true
      };

    case GET_CURRENCY_PAIRS_SUCCESS:
      return {
        ...state,
        loading: false,
        currencyPairs: action.payload
      };

    case GET_CURRENCY_PAIRS_FAILURE:
      return {
        ...state,
        loading: false
      };

    default:
      return state;
  }
};

export default currencyPairsReducer;
