import {createSelector} from 'reselect';
import {getPairTrend, getPairName} from '../utils';


const getCurrencyPairs = state => state.config.currencyPairs;
const getRates = state => state.rates;

const getViewRates = createSelector(
  getCurrencyPairs,
  getRates,
  (pairs, rates) => {
    if (!pairs) {
      return [];
    }

    const {cur, prev} = rates;
    return Object.keys(pairs).map(key => ({
      name: getPairName(pairs[key]),
      value: cur[key],
      trend: getPairTrend(cur[key], prev[key])
    }));
  }
);

export {getCurrencyPairs, getViewRates};
