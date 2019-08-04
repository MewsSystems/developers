// @flow strict

import { BASE_API_URL } from '../constants';
import type { CurrencyPairApi, Dispatch } from '../types';

import { sanitizeCurrencies, fetchCurrenciesConfigPending, fetchCurrenciesConfigError } from '.';

type RespError = {| error: Error |};
type RespSuccess = {| currencyPairs: CurrencyPairApi |};

type RespJson = RespError | RespSuccess;

const fetchCurrenciesConfig = () => {
  return async (dispatch: Dispatch) => {
    dispatch(fetchCurrenciesConfigPending());
    try {
      const response = await fetch(`${BASE_API_URL}/configuration`);
      const respJSON: RespJson = await response.json();

      if (respJSON.error) {
        dispatch(fetchCurrenciesConfigError({ error: respJSON.error }));
        return;
      }

      dispatch(sanitizeCurrencies({ currencyPairsApi: respJSON.currencyPairs }));
    } catch (error) {
      dispatch(fetchCurrenciesConfigError({ error }));
    }
  };
};

export default fetchCurrenciesConfig;
