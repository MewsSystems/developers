import { createSelector } from 'reselect';
import { prop } from 'ramda';

const currencyRates = prop('rates');

export const getRatesData = createSelector(
  [currencyRates],
  prop('rates')
);

export const getTrends = createSelector(
  [currencyRates],
  prop('trends')
);

export const getFilters = createSelector(
  [currencyRates],
  prop('filters')
);
