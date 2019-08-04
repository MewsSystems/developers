// @flow strict

import type { State } from './types';

export const INITIAL_STATE: State = {
  currencyPairs: [],
  filteredCurrencyPairs: [],
  isLoadingConfig: false,
  fetchConfigError: null,
};

export const ACTIONS = {
  SANITIZE_CURRENCIES: 'SANITIZE_CURRENCIES',
  FETCH_CURRENCIES_CONFIG_PENDING: 'FETCH_CURRENCIES_CONFIG_PENDING',
  FETCH_CURRENCIES_CONFIG_ERROR: 'FETCH_CURRENCIES_CONFIG_ERROR',
  ADD_FILTERED_CURRENCY_PAIR: 'ADD_FILTERED_CURRENCY_PAIR',
  REMOVE_FILTERED_CURRENCY_PAIR: 'REMOVE_FILTERED_CURRENCY_PAIR',
};

export const BASE_API_URL = 'http://localhost:3000';
