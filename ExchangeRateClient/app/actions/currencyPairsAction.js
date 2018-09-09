import {
  GET_CURRENCY_PAIRS_LOADING,
  GET_CURRENCY_PAIRS_SUCCESS,
  GET_CURRENCY_PAIRS_ERROR
} from './types';

export const getCurrencyPairsLoading = () => ({
  type: GET_CURRENCY_PAIRS_LOADING
});

export const getCurrencyPairsSuccess = currencyPairs => ({
  type: GET_CURRENCY_PAIRS_SUCCESS,
  payload: currencyPairs
});

export const getCurrencyPairsError = error => ({
  type: GET_CURRENCY_PAIRS_ERROR,
  payload: error
});
