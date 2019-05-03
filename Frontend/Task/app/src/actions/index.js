import { FETCH_CURRENCIES, FETCH_RATES, FETCHING_RATES_ERROR } from './types';
import currenciesApi from '../api/dataServer';

export const fetchCurrencies = () => (dispatch) => {
  currenciesApi.get('/configuration')
    .then((response) => {
      dispatch({ type: FETCH_CURRENCIES, payload: response.data.currencyPairs });
    }).catch(() => {
      /* Trying connect server */
      setTimeout(() => {
        fetchCurrencies()(dispatch);
      }, 2000);
    });
};

export const fetchRates = currenyKeyArray => (dispatch) => {
  currenciesApi.get('/rates', {
    params: {
      currencyPairIds: currenyKeyArray,
    },
  }).then((response) => {
    dispatch({ type: FETCH_RATES, payload: response.data.rates });
  }).catch((error) => {
    dispatch({ type: FETCHING_RATES_ERROR, payload: error.message });
  });
};
