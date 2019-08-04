// @flow strict

import type { State, Action } from '../types';

export const addFilteredCurrencyPair = (state: State, { payload }: Action): State => {
  const { filteredCurrencyPairId } = payload;

  if (!filteredCurrencyPairId) {
    return state;
  }

  return {
    ...state,
    filteredCurrencyPairs: [...state.filteredCurrencyPairs, filteredCurrencyPairId],
  };
};

export const removeFilteredCurrencyPair = (state: State, { payload }: Action): State => {
  const { filteredCurrencyPairId } = payload;
  if (!filteredCurrencyPairId) {
    return state;
  }

  const filteredCurrencyPairIndex = state.filteredCurrencyPairs.findIndex(
    id => id === filteredCurrencyPairId,
  );

  return {
    ...state,
    filteredCurrencyPairs: [
      ...state.filteredCurrencyPairs.slice(0, filteredCurrencyPairIndex),
      ...state.filteredCurrencyPairs.slice(filteredCurrencyPairIndex + 1),
    ],
  };
};
