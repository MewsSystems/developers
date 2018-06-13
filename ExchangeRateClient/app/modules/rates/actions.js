import createAction from "../../utils/createAction";

const API_URL = 'http://localhost:3000';

export const FETCH_CURRENCY_PAIRS_STARTED = 'FETCH_CURRENCY_PAIRS_STARTED';
export const FETCH_CURRENCY_PAIRS_SUCCEEDED = `FETCH_CURRENCY_PAIRS_SUCCEEDED`;
export const FETCH_CURRENCY_PAIRS_FAILED = `FETCH_CURRENCY_PAIRS_FAILED`;

export const fetchCurrencyPairs = () => dispatch => {
    dispatch(createAction(FETCH_CURRENCY_PAIRS_STARTED, {}));

    return fetch(`${API_URL}/configuration`)
        .then(response => response.json())
        .then(data => dispatch(createAction(FETCH_CURRENCY_PAIRS_SUCCEEDED, data)))
        .catch(error => dispatch(createAction(FETCH_CURRENCY_PAIRS_FAILED, error)))
};
