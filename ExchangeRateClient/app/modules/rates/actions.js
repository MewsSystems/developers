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

    // example url
    // http://localhost:3000/rates?currencyPairIds=70c6744c-cba2-5f4c-8a06-0dac0c4e43a1&currencyPairIds=41cae0fd-b74d-5304-a45c-ba000471eabd

    return fetch(`${API_URL}/rates` + query)
    // return fetch(`${API_URL}/rates?currencyPairIds=70c6744c-cba2-5f4c-8a06-0dac0c4e43a1&currencyPairIds=41cae0fd-b74d-5304-a45c-ba000471eabd&currencyPairIds=5b428ac9-ec57-513d-8a08-20199469fb4d&currencyPairIds=5b98842f-bfe5-5564-b321-068763d7e2a3&currencyPairIds=61fb0e0d-626e-5e0a-831a-ef95d5c32203&currencyPairIds=1993f7b9-f9be-551a-beac-312d6befd0cf&currencyPairIds=611398c5-6bd9-596e-8803-3ed0b093995d&currencyPairIds=a2bda952-4369-5d41-9d0b-e6c9947e5b82&currencyPairIds=b7fdd67f-5051-58b7-a3c6-84f5da637df5&currencyPairIds=f816e384-0e43-5ce7-a017-deaa8d666774`)
        .then(response => response.json())
        .then(data => dispatch(createAction(FETCH_CURRENCY_RATES_SUCCEEDED, data)))
        .catch(error => dispatch(createAction(FETCH_CURRENCY_RATES_FAILED, error)));
};
