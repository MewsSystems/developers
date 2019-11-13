import {getConfigurationUrl, getPairsUrl} from "../constants/ExchangeRateConstants";
import {RECEIVE_CURRENCY_PAIRS_CONFIG} from "../reducers/ConfigReducer";
import {RECEIVE_CURRENCY_PAIRS_VALUES} from "../reducers/CurrencyPairsValuesReducer";
import {RECEIVE_FILTER} from "../reducers/FilterReducer";

const receiveCurrencyPairsConfig = config => {
    return {
        type: RECEIVE_CURRENCY_PAIRS_CONFIG,
        config
    }
};

const receiveCurrencyPairsValues = row => {
    return {
        type: RECEIVE_CURRENCY_PAIRS_VALUES,
        row
    }
};

const receiveFilter = filter => {
    return {
        type: RECEIVE_FILTER,
        filter
    }
};

export const setFilter = filter => (dispatch, getState) => {
    dispatch(receiveFilter(filter));
    saveLocalState(getState().config, filter);
};

export const fetchConfig = () => (dispatch, getState) => {
    fetch(getConfigurationUrl(), {
        method: 'GET'
    }).then(response => {
        if (response.status === 200) {
            response.json().then(data => {
                dispatch(receiveCurrencyPairsConfig(data.currencyPairs));
                saveLocalState(data.currencyPairs, getState().filter);
            });
        }
    }).catch(error => {
        console.log(error);
    });
};

export const fetchCurrencyPairsValues = (pairsCodes) => dispatch => {
    fetch(getPairsUrl(pairsCodes), {
        method: 'GET'
    }).then(response => {
        if (response.status === 200) {
            response.json().then(data => {
                dispatch(receiveCurrencyPairsValues(data.rates));
            });
        }
    }).catch(error => {
        console.log(error);
    });
};

const saveLocalState = (config, filter) => {
    localStorage.setItem('ExchangeRateAppLocalState', JSON.stringify({config, filter}));
};