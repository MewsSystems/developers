import * as actionTypes from './actionTypes';

export const fetchRatesInit = rates => {
  return {
    type: actionTypes.FETCH_RATES_INIT,
    payload: rates
  };
};

export const fetchRatesSuccess = rates => {
  return {
    type: actionTypes.FETCH_RATES_SUCCESS,
    payload: rates
  };
};

export const updateRates = rates => {
  return {
    type: actionTypes.UPDATE_RATES,
    payload: rates
  };
};

export const fetchRatesFail = error => {
  return {
    type: actionTypes.FETCH_RATES_FAIL,
    payload: error
  };
};
