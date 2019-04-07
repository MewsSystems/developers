import { fetchApi } from './api';
import * as Endpoints from '../constants/endpoints';
import { RATES } from '../constants/actionTypes';

/**
 * Gets initial app configuration
 * @returns {function(*)}
 */
export function getRates(params) {
  return dispatch => dispatch(fetchApi(
    Endpoints.RATES,
    RATES,
    'GET',
    { currencyPairIds: [params] },
  ));
}

export function setRates(rates) {
  return {
    type: `SET_${RATES}`,
    payload: rates,
  };
}
