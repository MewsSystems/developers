import axios from 'axios';
import api from 'API';

import {
  GET_CURRENCY_PAIRS_STARTED,
  GET_CURRENCY_PAIRS_SUCCESS,
  GET_CURRENCY_PAIRS_FAILURE
} from './types';

const getCurrencyPairsStarted = () => ({
  type: GET_CURRENCY_PAIRS_STARTED
});

const getCurrencyPairsSuccess = pairs => ({
  type: GET_CURRENCY_PAIRS_SUCCESS,
  payload: {
    ...pairs
  }
});

const getCurrencyPairsFailure = error => ({
  type: GET_CURRENCY_PAIRS_FAILURE,
  payload: {
    error
  }
});

export const getCurrencyPairs = () => {
  return dispatch => {
    dispatch(getCurrencyPairsStarted());

    return axios
      .get(api.CURRENCY_PAIRS)
      .then(res => {
        dispatch(getCurrencyPairsSuccess(res.data.currencyPairs));
      })
      .catch(err => {
        dispatch(getCurrencyPairsFailure(err.message));
      });
  };
};
