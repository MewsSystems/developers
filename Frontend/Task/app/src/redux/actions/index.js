// @flow strict

import type { CurrencyPairApi, Action, RatesApi } from '../types';
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

export const addFilteredCurrencyPair = (payload: {| filteredCurrencyPairId: string |}) => ({
  type: ACTIONS.ADD_FILTERED_CURRENCY_PAIR,
  payload,
});

export const removeFilteredCurrencyPair = (payload: {| filteredCurrencyPairId: string |}) => ({
  type: ACTIONS.REMOVE_FILTERED_CURRENCY_PAIR,
  payload,
});

export const fetchRatesPending = (): Action => ({
  type: ACTIONS.FETCH_RATES_PENDING,
  payload: {},
});

export const fetchRatesError = (payload: {| error: Error |}): Action => ({
  type: ACTIONS.FETCH_RATES_ERROR,
  payload,
});

export const addRates = (payload: {| ratesApi: RatesApi |}): Action => ({
  type: ACTIONS.ADD_RATES,
  payload,
});
