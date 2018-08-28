import { 
  SET_CURRENCY_PAIRS,
  SET_RATES,
  SELECT_PAIR
} from "./constants";

export const setCurrencyPairs = pairs => ({ 
  type: SET_CURRENCY_PAIRS, 
  payload: pairs 
});

export const setRates = rates => ({ 
  type: SET_RATES, 
  payload: rates 
});

export const selectPair = pair => ({
  type: SELECT_PAIR,
  payload: pair
})