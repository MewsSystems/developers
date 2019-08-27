import * as types from './types';

export function getConfigurationSuccess(configuration) {
  return {
    type: types.GET_CONFIGURATION_SUCCESS,
    configuration,
  };
}
export function getRateSuccess(rates) {
  return {
    type: types.GET_RATE_SUCCESS,
    rates,
	};
}
export function requestError(error) {
  return {
    type: types.REQUEST_ERROR,
    error,
	};
}

export function responseError(status) {
  return {
    type: types.RESPONSE_ERROR,
    status,
	};
}

export function filter(rateId) {
  return {
    type: types.FILTER,
    rateId,
	};
}
