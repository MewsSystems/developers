// @flow strict

import type { CurrencyPairApi, Action } from '../types';
import { ACTIONS } from '../constants';

export const sanitizeCurrencies = (payload: {| currencyPairsApi: CurrencyPairApi |}): Action => ({
  type: ACTIONS.SANITIZE_CURRENCIES,
  payload,
});
