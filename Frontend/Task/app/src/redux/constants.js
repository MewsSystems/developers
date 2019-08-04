// @flow strict

import type { State } from './types';

export const INITIAL_STATE: State = {
  currencyPairs: [],
  filteredCurrencies: [],
  isLoadingConfig: false,
  fetchConfigError: null,
};

export const ACTIONS = {
  SANITIZE_CURRENCIES: 'SANITIZE_CURRENCIES',
  FETCH_CURRENCIES_CONFIG_PENDING: 'FETCH_CURRENCIES_CONFIG_PENDING',
  FETCH_CURRENCIES_CONFIG_ERROR: 'FETCH_CURRENCIES_CONFIG_ERROR',
};

export const BASE_API_URL = 'http://localhost:3000';
