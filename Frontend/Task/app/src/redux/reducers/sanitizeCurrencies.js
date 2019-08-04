// @flow strict

import type { State, Action } from '../types';

const sanitizeCurrencies = (state: State, { payload }: Action): State => {
  const { currencyPairsApi } = payload;
  if (!currencyPairsApi) {
    return state;
  }

  const currencyPairs = Object.keys(currencyPairsApi).map(key => ({
    id: key,
    currencies: currencyPairsApi[key],
  }));

  return {
    ...state,
    currencyPairs,
  };
};

export default sanitizeCurrencies;
