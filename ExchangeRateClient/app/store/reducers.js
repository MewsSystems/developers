import {
  SET_CURRENCY_PAIRS,
  SET_RATES,
  SET_TOGGLE
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
    case SET_TOGGLE:
      return {
        ...state,
        togglePairs: action.togglePairs
      }
    default:
      return state;
  }
};

export default reducers;