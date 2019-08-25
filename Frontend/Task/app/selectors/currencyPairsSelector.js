import { createSelector } from 'reselect';
import { prop, keys } from 'ramda';

const currencyPairs = prop('pairs');

export const fetchingCurrenyPairs = createSelector(
  [currencyPairs],
  prop('loading')
);

export const getPairs = createSelector(
  [currencyPairs],
  prop('currencyPairs')
);

const pairsByIds = createSelector(
  [currencyPairs],
  prop('currencyPairs')
);

export const getPairsIds = createSelector(
  [pairsByIds],
  keys
);
