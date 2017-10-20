import {createSelector} from 'reselect';
import {getPairTrend, getPairName} from '../utils';

const getIsWatching = state => state.isWatching;
const getCurrencyPairs = state => state.config.currencyPairs || {};
const getFilter = state => state.filter;
const getRates = state => state.rates;

const getViewPairs = createSelector(
  getCurrencyPairs,
  pairs => pairs ? Object.keys(pairs).map(key => ({
    id: key,
    name: getPairName(pairs[key]),
  })) : []
);

const getViewRates = createSelector(
  getViewPairs,
  getRates,
  (pairs, {cur, prev}) => pairs.map(pair => ({
    ...pair,
    value: cur[pair.id],
    trend: getPairTrend(cur[pair.id], prev[pair.id])
  }))
);

const getFilteredRates = createSelector(
  getViewRates,
  getFilter,
  (rates, filter) => rates.filter(rate => !~filter.indexOf(rate.id))
);

export {getCurrencyPairs, getFilter, getViewPairs, getFilteredRates, getIsWatching};
