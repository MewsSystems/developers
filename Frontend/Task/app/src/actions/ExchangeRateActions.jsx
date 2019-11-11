import {getConfigurationUrl, getPairsUrl} from "../constants/ExchangeRateConstants";
import {RECEIVE_CURRENCY_PAIRS_CONFIG} from "../reducers/ConfigReducer";
import {RECEIVE_CURRENCY_PAIRS_VALUES} from "../reducers/CurrencyPairsValuesReducer";
import {RECEIVE_FILTER} from "../reducers/FilterReducer";

const receiveCurrencyPairsConfig = (config) => {
    return {
        type: RECEIVE_CURRENCY_PAIRS_CONFIG,
        config
    }
};

const receiveCurrencyPairsValues = (row) => {
    return {
        type: RECEIVE_CURRENCY_PAIRS_VALUES,
        row
    }
};

export const setFilter = (filter) => {
    return {
        type: RECEIVE_FILTER,
        filter
    }
};

export const fetchConfig = () => dispatch => {
    fetch(getConfigurationUrl(), {
        method: 'GET'
    }).then(response => {
        if (response.status === 200) {
            response.json().then(data => {
                dispatch(receiveCurrencyPairsConfig(data.currencyPairs))
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
        } else {
            console.log('errorr');
        }
    }).catch(error => {
        console.log(error);
    });
};