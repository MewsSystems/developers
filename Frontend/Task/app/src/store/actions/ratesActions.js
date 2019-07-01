import * as actionTypes from './actionTypes';

export const fetchRatesInit = rates => {
  return {
    type: actionTypes.FETCH_RATES_INIT,
    payload: rates
  };
};

export const updateRatesSuccess = rates => {
  return {
    type: actionTypes.UPDATE_RATES_SUCCESS,
    payload: rates
  };
};

export const updateRates = () => {
  return {
    type: actionTypes.UPDATE_RATES
  };
};

export const fetchRatesFail = error => {
  return {
    type: actionTypes.FETCH_RATES_FAIL,
    payload: error
  };
};

export const fetchRatesRetry = () => {
  return {
    type: actionTypes.FETCH_RATES_RETRY
  };
};
