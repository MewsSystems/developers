import { STORE_PAIRS, UPDATE_RATES } from '../constants';

function storePairs(pairs: Object) {
  return {
    type: STORE_PAIRS,
    pairs,
  };
}
function storeRates(rateData: Object) {
  return {
    type: UPDATE_RATES,
    rateData,
  };
}

export { storePairs, storeRates };
