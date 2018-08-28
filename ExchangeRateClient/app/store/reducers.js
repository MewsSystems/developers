import {
  SET_CURRENCY_PAIRS,
  SET_RATES,
  SELECT_PAIR
} from './constants';

const initialState = {};

const reducers = (state = initialState, action) => {
  const { type, payload } = action

  switch (type) {
    case SET_CURRENCY_PAIRS:
      return { 
        ...state, 
        currencyPairs: payload
      };
    case SET_RATES:
      return {
        ...state,
        rates: payload
      };
    case SELECT_PAIR:
      return {
        ...state,
        selected: payload
      }
    default:
      return state;
  }
};

export default reducers;