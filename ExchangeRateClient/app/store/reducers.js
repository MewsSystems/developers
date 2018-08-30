import {
  SET_CURRENCY_PAIRS,
  SET_RATES,
} from './constants';

const initialState = {};

const reducers = (state = initialState, action) => {

  switch (action.type) {
    case SET_CURRENCY_PAIRS:
      return { 
        ...state, 
        currencyPairs: action.pairs
      };
    case SET_RATES:
      return {
        ...state,
        ...state.currencyPairs[action.pair][2] = { 
          rate: action.rate 
        }
      };
    default:
      return state;
  }
};

export default reducers;