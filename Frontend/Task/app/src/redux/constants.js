// @flow strict

import type { State } from './types';

export const INITIAL_STATE: State = {
  currencyPairs: [],
  filteredCurrencyPairs: [],
  isLoadingConfig: false,
  fetchConfigError: null,
  fetchRatesError: null,
  isLoadingRates: false,
};

export const ACTIONS = {
  SANITIZE_CURRENCIES: 'SANITIZE_CURRENCIES',
  FETCH_CURRENCIES_CONFIG_PENDING: 'FETCH_CURRENCIES_CONFIG_PENDING',
  FETCH_CURRENCIES_CONFIG_ERROR: 'FETCH_CURRENCIES_CONFIG_ERROR',
  ADD_FILTERED_CURRENCY_PAIR: 'ADD_FILTERED_CURRENCY_PAIR',
  REMOVE_FILTERED_CURRENCY_PAIR: 'REMOVE_FILTERED_CURRENCY_PAIR',
  FETCH_RATES_PENDING: 'FETCH_RATES_PENDING',
  FETCH_RATES_ERROR: 'FETCH_RATES_ERROR',
  ADD_RATES: 'ADD_RATES',
};

export const BASE_API_URL = 'http://localhost:3000';
