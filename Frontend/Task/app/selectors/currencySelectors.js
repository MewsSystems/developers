import { createSelector } from 'reselect';

const calculateTrend = (previousRate, currentRate) => {
  if (currentRate > previousRate) {
    return 'growing';
  } else if (currentRate < previousRate) {
    return 'declining';
  }
  else {
    return 'stagnating';
  }
}

const selectPairs = (state) => state.configuration.currencyPairs;
const selectRates = (state) => state.rates.rates;
const selectFilter = (state) => state.rates.filter;

export const selectCurrencyPairsIds = createSelector(
  selectPairs,
  (currencyPairs) => (Object.keys(currencyPairs))
);

export const selectCurrencyRates = createSelector(
  selectCurrencyPairsIds,
  selectPairs,
  selectRates,
  (pairIds, pairs, rates) => {
    return pairIds.map((pairId) => {
      const currencyPair = pairs[pairId];
      const displayName = `${currencyPair[0].code} / ${currencyPair[1].code}`;

      const currencyRate = rates[pairId];
      const currentRate = (currencyRate) ? currencyRate.currentRate : null;
      const trend = (currencyRate) ? calculateTrend(currencyRate.previousRate, currentRate) : null;

      return {
        displayName,
        currentRate,
        trend
      }
    });
  }
);

export const selectFilteredRates = createSelector(
  selectCurrencyRates,
  selectFilter,
  (currencyRates, filter) => currencyRates.filter((rate) => (filter && filter !== '') ? rate.displayName.toLowerCase().search(filter) > -1 : true)
);