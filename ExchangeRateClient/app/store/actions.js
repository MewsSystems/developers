import { 
  SET_CURRENCY_PAIRS,
  SET_RATES,
  SELECT_PAIR
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