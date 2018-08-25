// @flow
import { STORE_PAIRS, UPDATE_RATES } from '../constants';

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

export { storePairs, storeRates };
