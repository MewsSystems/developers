// @flow strict

import type { State, Action } from '../types';

export const setRatesError = (state: State, { payload }: Action): State => {
  const { error } = payload;
  if (!error) {
    return state;
  }

  return {
    ...state,
    isLoadingRates: false,
    fetchRatesError: error,
  };
};

export const setRatesLoading = (state: State): State => {
  return { ...state, isLoadingRates: true };
};

export const addRates = (state: State, { payload }: Action): State => {
  const { ratesApi } = payload;

  if (!ratesApi) {
    return state;
  }

  const currencyPairs = state.currencyPairs.map(currencyPair => {
    const rate = (Object.entries(ratesApi).find(([key]) => {
      return key === currencyPair.id;
    }) || [])[1];

    if (!rate) {
      return currencyPair;
    }

    if (typeof rate !== 'number') {
      throw new Error('Invalid API type, number was expected');
    }

    return {
      ...currencyPair,
      rates: [...currencyPair.rates, rate],
    };
  });

  return {
    ...state,
    currencyPairs,
    isLoadingRates: false,
  };
};
