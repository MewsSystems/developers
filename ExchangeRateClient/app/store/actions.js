import { 
  SET_CURRENCY_PAIRS,
  SET_RATES,
  SET_TOGGLE
} from "./constants";

export const setCurrencyPairs = pairs => ({ 
  type: SET_CURRENCY_PAIRS, 
  pairs 
});

export const setRates = (rate, pair) => ({ 
  type: SET_RATES, 
  rate,
  pair
});

export const setTogglePairs = togglePairs => ({
  type: SET_TOGGLE,
  togglePairs
})