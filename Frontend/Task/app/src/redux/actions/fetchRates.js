// @flow strict

import { BASE_API_URL } from '../constants';
import type { RatesApi, Dispatch } from '../types';

import { addRates, fetchRatesError, fetchRatesPending } from '.';

type RespError = {| error: Error |};
type RespSuccess = {| rates: RatesApi |};

type RespJson = RespError | RespSuccess;

const fetchRates = (query: string) => {
  return async (dispatch: Dispatch) => {
    dispatch(fetchRatesPending());

    try {
      const response = await fetch(`${BASE_API_URL}/rates?${query}`);
      const respJSON: RespJson = await response.json();

      if (respJSON.error) {
        dispatch(fetchRatesError({ error: respJSON.error }));
        return;
      }

      dispatch(addRates({ ratesApi: respJSON.rates }));
    } catch (error) {
      dispatch(fetchRatesError({ error }));
    }
  };
};

export default fetchRates;
