import {
  API_ERROR_CONFIGURATION,
  API_ERROR_RATES,
  API_RECEIVED_CONFIGURATION,
  API_RECEIVED_RATES,
  API_REQUESTED_CONFIGURATION,
  API_REQUESTED_RATES,
  UI_CURRENCY_FILTER,
  UI_CURRENCY_SELECT,
  UI_CURRENCY_DESELECT,
} from './types';

// API actions
export const getConfigurationAction = endpoint => (dispatch) => {
  if (!endpoint) return null; // nothing to request
  dispatch({
    type: API_REQUESTED_CONFIGURATION,
  });
  return fetch(`${endpoint}/configuration`)
    .then(response => response.json())
    .then((json) => {
      dispatch({
        type: API_RECEIVED_CONFIGURATION,
        payload: json.currencyPairs,
      });
    })
    .catch((err) => {
      dispatch({
        type: API_ERROR_CONFIGURATION,
        payload: err.message,
      });
    });
};

export const getRatesAction = ({ endpoint, currencyPairIds }) => (dispatch) => {
  if (!endpoint || !currencyPairIds || currencyPairIds.length === 0) {
    return null; // nothing to request
  }
  dispatch({
    type: API_REQUESTED_RATES,
  });
  const queryString = currencyPairIds.map((id, index) => {
    return `currencyPairIds[${index.toString()}]=${id}`;
  }).join('&');
  return fetch(`${endpoint}/rates?${queryString}`)
    .then(response => response.json())
    .then((json) => {
      dispatch({
        type: API_RECEIVED_RATES,
        payload: json.rates,
      });
    })
    .catch((err) => {
      dispatch({
        type: API_ERROR_RATES,
        payload: err.message,
      });
    });
};

// UI actions
export const setCurrencyFilterAction = currencyFilter => ({
  type: UI_CURRENCY_FILTER,
  payload: currencyFilter,
});

export const selectCurrencyPairAction = currencyPairId => ({
  type: UI_CURRENCY_SELECT,
  payload: currencyPairId,
});

export const deselectCurrencyPairAction = currencyPairId => ({
  type: UI_CURRENCY_DESELECT,
  payload: currencyPairId,
});
