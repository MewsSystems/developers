// @flow strict

import type { State } from './types';

export const INITIAL_STATE: State = {
  currencyPairs: [],
  filteredCurrencies: [],
};

export const ACTIONS = {
  SANITIZE_CURRENCIES: 'SANITIZE_CURRENCIES',
};
