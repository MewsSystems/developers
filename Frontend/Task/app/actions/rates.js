import axios from 'axios';
import { notify } from 'reapop';
import api from 'API';
import { calculateTrends } from 'Utils';
import { getRatesData } from 'Selectors';

import {
  GET_RATES_STARTED,
  GET_RATES_SUCCESS,
  GET_RATES_FAILURE,
  COMPUTE_TRENDS
} from 'Actions/types';

const errorNotification = {
  title: '500 Internal Server Error',
  message: 'Server cannot process the request',
  dismissAfter: 4000,
  status: 500
};

const getRatesStarted = () => ({
  type: GET_RATES_STARTED
});

const getRatesSuccess = rates => ({
  type: GET_RATES_SUCCESS,
  payload: {
    ...rates
  }
});

const getRatesFailure = error => ({
  type: GET_RATES_FAILURE,
  payload: {
    error
  }
});

const computeTrends = (oldRates, newRates) => ({
  type: COMPUTE_TRENDS,
  payload: calculateTrends(oldRates, newRates)
});

export const getRates = pairIds => {
  return (dispatch, getState) => {
    dispatch(getRatesStarted());

    axios
      .get(api.RATES, {
        params: {
          currencyPairIds: pairIds
        }
      })
      .then(res => {
        const oldRates = getRatesData(getState());
        dispatch(getRatesSuccess(res.data.rates));
        const newRates = getRatesData(getState());
        dispatch(computeTrends(oldRates, newRates));
      })
      .catch(err => {
        dispatch(notify(errorNotification));
        dispatch(getRatesFailure(err.message));
      });
  };
};
