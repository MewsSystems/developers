// @flow
import { createSelector } from 'reselect';

const getCurrencyPairMap = state => state.getIn(['app', 'currencyPairs']);

export const getCurrencyPairList = createSelector(getCurrencyPairMap, pairs =>
  pairs.toList()
);

const getRateMap = state => state.getIn(['app', 'rates']);

export const getCurrencyPairRates = createSelector(
  [getCurrencyPairMap, getRateMap],
  (currencyPairs, rates) =>
    rates
      .map((rate, id) => ({ id, rate, currencyPair: currencyPairs.get(id) }))
      .toList()
);
