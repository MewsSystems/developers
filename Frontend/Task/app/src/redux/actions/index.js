// @flow strict

import type { CurrencyPairApi, Action } from '../types';
import { ACTIONS } from '../constants';

export const sanitizeCurrencies = (payload: {| currencyPairsApi: CurrencyPairApi |}): Action => ({
  type: ACTIONS.SANITIZE_CURRENCIES,
  payload,
});

export const fetchCurrenciesConfigPending = (): Action => ({
  type: ACTIONS.FETCH_CURRENCIES_CONFIG_PENDING,
  payload: {},
});

export const fetchCurrenciesConfigError = (payload: {| error: Error |}): Action => ({
  type: ACTIONS.FETCH_CURRENCIES_CONFIG_ERROR,
  payload,
});
