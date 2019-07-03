import * as actionTypes from '../actions/actionTypes';
import { RatesInterface } from '../../types';
import {
  fetchRatesFailInterface,
  updateRatesSuccessInterface
} from '../actions/types';

const initialState = {};

const updateRates = (
  state: RatesInterface,
  action: updateRatesSuccessInterface
) => {
  const newState = action.payload;
  const keys = Object.keys(action.payload);
  const setTrend = (prev: RatesInterface, next: RatesInterface) => {
    if (prev > next) {
      return 'declining';
    }
    if (prev < next) {
      return 'growing';
    }
    if (prev === next) {
      return 'stagnating';
    }
  };
  let trend: string;
  let rate: number;
  keys.forEach(key => {
    rate = newState[key];
    state[key] !== undefined
      ? (trend = setTrend(state[key].rate, newState[key]))
      : (trend = 'stagnating');
    return (newState[key] = {
      rate,
      trend
    });
  });
  return newState;
};
const fetchFails = (state: RatesInterface) => {
  return state;
};
const reducer = (
  state: RatesInterface = initialState,
  action: updateRatesSuccessInterface | fetchRatesFailInterface
) => {
  switch (action.type) {
    case actionTypes.UPDATE_RATES_SUCCESS:
      return updateRates(state, action);
    case actionTypes.FETCH_RATES_FAIL:
      return fetchFails(state);
    default:
      return state;
  }
};

export default reducer;
