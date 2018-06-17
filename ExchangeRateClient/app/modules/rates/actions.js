import createAction from '../../utils/createAction';

const API_URL = 'http://localhost:3000';

export const FETCH_CURRENCY_PAIRS_STARTED = 'FETCH_CURRENCY_PAIRS_STARTED';
export const FETCH_CURRENCY_PAIRS_SUCCEEDED = `FETCH_CURRENCY_PAIRS_SUCCEEDED`;
export const FETCH_CURRENCY_PAIRS_FAILED = `FETCH_CURRENCY_PAIRS_FAILED`;

export const FETCH_CURRENCY_RATES_STARTED = 'FETCH_CURRENCY_RATES_STARTED';
export const FETCH_CURRENCY_RATES_SUCCEEDED = `FETCH_CURRENCY_RATES_SUCCEEDED`;
export const FETCH_CURRENCY_RATES_FAILED = `FETCH_CURRENCY_RATES_FAILED`;

export const fetchCurrencyPairs = () => dispatch => {
    dispatch(createAction(FETCH_CURRENCY_PAIRS_STARTED, {}));

    return fetch(`${API_URL}/configuration`)
        .then(response => response.json())
        .then(data => dispatch(createAction(FETCH_CURRENCY_PAIRS_SUCCEEDED, data)))
        .catch(error => dispatch(createAction(FETCH_CURRENCY_PAIRS_FAILED, error)));
};

export const fetchCurrencyRates = (array) => dispatch => {
    dispatch(createAction(FETCH_CURRENCY_RATES_STARTED, {}));

    let query = '?currencyPairIds=' + array.join('&currencyPairIds=');

    // correct url
    // http://localhost:3000/rates?currencyPairIds=70c6744c-cba2-5f4c-8a06-0dac0c4e43a1&currencyPairIds=41cae0fd-b74d-5304-a45c-ba000471eabd

    return fetch(`${API_URL}/rates` + query)
        .then(response => response.json())
        .then(data => dispatch(createAction(FETCH_CURRENCY_RATES_SUCCEEDED, data)))
        .catch(error => dispatch(createAction(FETCH_CURRENCY_RATES_FAILED, error)));
};
