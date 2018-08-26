// @flow
import { STORE_PAIRS, UPDATE_RATES, UPDATE_TRENDS } from '../constants';

function storePairs(pairData: Object) {
  return {
    type: STORE_PAIRS,
    pairData,
  };
}
function storeRates(rateData: Object) {
  return {
    type: UPDATE_RATES,
    rateData,
  };
}
function updateTrendData(exchangeID: string, exchangeRate: number) {
  return {
    type: UPDATE_TRENDS,
    exchangeID,
    exchangeRate,
  };
}

export { storePairs, storeRates, updateTrendData };
