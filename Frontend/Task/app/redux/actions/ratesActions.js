export const ratesActions = {
  UPDATE_RATES: 'rates/UPDATE_RATES',
  UPDATE_RATES_SUCCESS: 'rates/UPDATE_RATES_SUCCESS',
  UPDATE_RATES_FAILED: 'rates/UPDATE_RATES_FAILED',

  FILTER_RATES: 'rates/FILTER_RATES'
};

export const ratesActionCreators = {
  updateRates: () => ({ type: ratesActions.UPDATE_RATES }),
  updateRatesSuccess: (rates) => ({ type: ratesActions.UPDATE_RATES_SUCCESS, payload: { ...rates } }),
  updateRatesFailed: () => ({ type: ratesActions.UPDATE_RATES_FAILED }),

  filterRates: (filter) => ({ type: ratesActions.FILTER_RATES, payload: filter })
};