// @flow
export const actionTypes = {
  FETCH_CONFIG: 'exchange-rates/FETCH_CONFIG',
  FETCH_CONFIG_FULFILLED: 'exchange-rates/FETCH_CONFIG_FULFILLED',
  FETCH_CONFIG_FAILED: 'exchange-rates/FETCH_CONFIG_FAILED',
  FETCH_CONFIG_CANCELLED: 'exchange-rates/FETCH_CONFIG_CANCELLED',

  FETCH_RATES: 'exchange-rates/FETCH_RATES',
  FETCH_RATES_FULFILLED: 'exchange-rates/FETCH_RATES_FULFILLED',
  FETCH_RATES_FAILED: 'exchange-rates/FETCH_RATES_FAILED',
  FETCH_RATES_CANCELLED: 'exchange-rates/FETCH_RATES_CANCELLED',
};

export function fetchConfig() {
  return { type: actionTypes.FETCH_CONFIG };
}

export function fetchConfigFulfilled(payload: Object) {
  return {
    type: actionTypes.FETCH_CONFIG_FULFILLED,
    payload,
  };
}

export function fetchConfigFailed(payload: Error) {
  return {
    type: actionTypes.FETCH_CONFIG_FAILED,
    error: true,
    payload,
  }
}

export function fetchRates(interval: number, ids: Array<string>) {
  return {
    type: actionTypes.FETCH_RATES,
    payload: { interval, ids },
  };
}

export function fetchRatesFulfilled(payload: Object) {
  return {
    type: actionTypes.FETCH_RATES_FULFILLED,
    payload,
  };
}

export function fetchRatesFailed(currencyPaidIds: Array<string>, data: Object) {
  return {
    type: actionTypes.FETCH_RATES_FAILED,
    error: true,
    payload: { currencyPaidIds, data },
  };
}
